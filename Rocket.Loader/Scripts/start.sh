#!/bin/bash
# This script starts a Unturned 3 server on Linux machines
# Syntax: start.sh <instance name>
# Author: fr34kyn01535

INSTANCE_NAME=$1
UNTURNED_HOME="./unturned"

ulimit -n 2048
$UNTURNED_HOME/Unturned.x86 -nographics -batchmode -logfile "$UNTURNED_HOME/unturned.log" +secureserver/$INSTANCE_NAME & disown && tail -f "$UNTURNED_HOME/unturned.log"
