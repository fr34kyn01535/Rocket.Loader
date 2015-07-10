#!/bin/bash
# This script starts a Unturned 3 server on Linux machines
# Syntax: start.sh <instance name>
# Author: fr34kyn01535

INSTANCE_NAME=$1
UNTURNED_HOME="./unturned"

ulimit -n 2048
cd $UNTURNED_HOME
if [ ! -f $UNTURNED_HOME/RocketLauncher.exe ]; then
	if [ -f "RocketLauncher.exe" ]; then
		mv RocketLauncher.exe $UNTURNED_HOME/RocketLauncher.exe
	fi
fi

if [ -f $UNTURNED_HOME/RocketLauncher.exe ]; then
	mono RocketLauncher.exe $UNTURNED_HOME
else
	echo "RocketLauncher not found"
fi
