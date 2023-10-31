FROM ubuntu:20.04

WORKDIR /vcloud

RUN apt-get update && DEBIAN_FRONTEND=noninteractive TZ=Europe/Budapest apt-get install --no-install-recommends -yqq \
    sudo \
    curl git git-svn \
    ssh openssh-server \
    python3 \
    openjdk-11-jdk openjdk-11-doc openjdk-11-source \
    ant ant-optional rsync

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