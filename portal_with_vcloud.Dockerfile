FROM ubuntu:20.04 AS base

WORKDIR /vcloud

RUN apt-get update && DEBIAN_FRONTEND=noninteractive TZ=Europe/Budapest apt-get install --no-install-recommends -yqq \
    sudo \
    curl git git-svn \
    ssh openssh-server \
    python3 \
    openjdk-11-jdk openjdk-11-doc openjdk-11-source \
    ant ant-optional rsync \
    wget 

RUN wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb

RUN apt-get update && DEBIAN_FRONTEND=noninteractive TZ=Europe/Budapest apt-get -y install aspnetcore-runtime-7.0
RUN apt-get -y install dotnet-sdk-7.0

RUN useradd -m vcloud
RUN echo 'vcloud ALL=(ALL) NOPASSWD:ALL' >> /etc/sudoers
RUN chown vcloud:vcloud . -R
USER vcloud

COPY verifiercloud verifiercloud
COPY .git .git
COPY .gitattributes .gitignore .gitmodules ./
RUN sudo chown vcloud:vcloud verifiercloud -R
RUN git config --global --add safe.directory /vcloud/verifiercloud
WORKDIR /vcloud/verifiercloud
RUN ant jar-big

COPY id_rsa_master /home/vcloud/.ssh/id_rsa
COPY id_rsa_master.pub /home/vcloud/.ssh/id_rsa.pub
RUN sudo chown vcloud:vcloud /home/vcloud/.ssh -R
RUN sudo chmod 600 /home/vcloud/.ssh/id_rsa

COPY verifiercloudconfig /home/vcloud/.verifiercloud
RUN sudo chown vcloud:vcloud /home/vcloud/.verifiercloud -R

WORKDIR /vcloud/benchmarkingportal/src
COPY BenchmarkingPortal.Bll ./BenchmarkingPortal.Bll
COPY BenchmarkingPortal.Dal ./BenchmarkingPortal.Dal
COPY BenchmarkingPortal.Web ./BenchmarkingPortal.Web
COPY BenchmarkingPortal.Migrations.Base ./BenchmarkingPortal.Migrations.Base
COPY BenchmarkingPortal.Migrations.Test ./BenchmarkingPortal.Migrations.Test
COPY BenchmarkingPortal.Test ./BenchmarkingPortal.Test
COPY BenchmarkingPortal.sln BenchmarkingPortal.pfx ./

RUN sudo chown vcloud:vcloud /vcloud/benchmarkingportal -R

# Restore as distinct layers
RUN dotnet restore ./BenchmarkingPortal.Web/BenchmarkingPortal.Web.csproj
RUN dotnet build ./BenchmarkingPortal.Web/BenchmarkingPortal.Web.csproj -c Release -o ../build
RUN dotnet publish ./BenchmarkingPortal.Web/BenchmarkingPortal.Web.csproj -c Release -o ../publish

COPY BenchmarkingPortal.pfx ../publish/

EXPOSE 80
EXPOSE 443
WORKDIR /vcloud/benchmarkingportal/publish
ENTRYPOINT ["dotnet", "BenchmarkingPortal.Web.dll"]