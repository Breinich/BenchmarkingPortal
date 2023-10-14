1. Clone the verifiercloud repo into the folder `verifiercloud`, with `git/cpachecker` also populated

2. `docker-compose build`

3. `docker-compose run`

Expected output: 

``` bash
$ sudo docker-compose up              
[+] Building 0.0s (0/0)                                                                                                                                                                    
[+] Running 3/2
 ✔ Container docker-master-1  Recreated                                                                                                                                               0.2s 
 ✔ Container docker-worker-1  Recreated                                                                                                                                               0.2s 
 ✔ Container docker-client-1  Recreated                                                                                                                                               0.1s 
Attaching to docker-client-1, docker-master-1, docker-worker-1
docker-master-1  | ssh: connect to host worker port 22: Connection refused
docker-worker-1  |  * Starting OpenBSD Secure Shell server sshd
docker-worker-1  |    ...done.
docker-master-1  | Warning: Permanently added 'worker,172.19.0.3' (ECDSA) to the list of known hosts.
docker-client-1  | Retrying..
docker-client-1  | Retrying..
docker-client-1  | Connection to Master established.
docker-client-1  | UNAUTHENTICATED@master> Now authorized as USER
docker-client-1  | USER@master> Received 1 of 3 run results.
docker-client-1  | Received 2 of 3 run results.
docker-client-1  | Received 3 of 3 run results.
docker-client-1  | Received all results.
docker-client-1  | USER@master> Shutting down EXIT_SUCCESS[0]
docker-client-1 exited with code 0
```

## How to use

### Example configuration

```

#!/bin/bash

 

echo "Enter the tool:"
read -e TOOL

 

echo "Enter the XML file path:"
read -e XML

 

echo "Enter the tool binary's folder (optional, press Enter to use the base directory of the tool):"
read -e TOOL_BIN

 

echo "Enter the output directory (optional, press Enter to use 'results/<toolname>/<timestamp>'):"
read -e OUTPUT

 

echo "Enter the priority (optional, press Enter for 'LOW' priority):"
read PRIO

 

# Set default values if the user didn't provide any
TOOL_BIN=${TOOL_BIN:=$TOOL}
OUTPUT=${OUTPUT:="results/$(basename "$TOOL")/$(date +"%Y-%m-%d_%H:%M:%S")"}
PRIO=${PRIO:="LOW"}

 

mkdir "$OUTPUT" -p
/benchexec/contrib/vcloud-benchmark.py --tryLessMemory --no-container "$XML" --tool-directory "$TOOL_BIN" --vcloudAdditionalFiles "$TOOL" -o "$OUTPUT" --vcloudPriority "$PRIO"

 

echo "to run again: "
echo /benchexec/contrib/vcloud-benchmark.py --tryLessMemory --no-container "$XML" --tool-directory "$TOOL_BIN" --vcloudAdditionalFiles "$TOOL" -o "$OUTPUT" --vcloudPriority "$PRIO"

```