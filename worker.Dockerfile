FROM ubuntu:20.04

WORKDIR /vcloud

RUN apt-get update && DEBIAN_FRONTEND=noninteractive TZ=Europe/Budapest apt-get install --no-install-recommends -yqq \
    sudo software-properties-common \
    curl git git-svn \
    ssh openssh-server \
    python \
    openjdk-11-jre rsync

RUN add-apt-repository ppa:sosy-lab/benchmarking
RUN apt install --no-install-recommends -yqq benchexec

RUN useradd -m vcloud
RUN echo 'vcloud ALL=(ALL) NOPASSWD:ALL' >> /etc/sudoers
RUN chown vcloud:vcloud . -R
USER vcloud

COPY id_rsa_master.pub /home/vcloud/.ssh/authorized_keys
RUN sudo chown vcloud:vcloud /home/vcloud/.ssh -R
RUN sudo chmod 700 /home/vcloud/.ssh

CMD ["/bin/bash", "-c", "sudo chmod o+wt /sys/fs/cgroup/blkio/system.slice/benchexec-cgroup.service /sys/fs/cgroup/cpu/system.slice/benchexec-cgroup.service /sys/fs/cgroup/cpuacct/system.slice/benchexec-cgroup.service /sys/fs/cgroup/cpuset/system.slice/benchexec-cgroup.service /sys/fs/cgroup/freezer/system.slice/benchexec-cgroup.service /sys/fs/cgroup/memory/system.slice/benchexec-cgroup.service; sudo /etc/init.d/ssh start; tail -f /dev/null"]
