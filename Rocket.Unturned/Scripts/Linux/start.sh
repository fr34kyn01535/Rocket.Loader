#!/bin/bash
# This script starts a Unturned 3 server on Linux machines
# Syntax: start.sh <instance name>
# Author: fr34kyn01535

# Before you start the Unturned 3 server on linux make sure the following packages are installed:
#htop unzip # Utils                                                                           
#screen Xorg xinit x11-common # Headless X server
#libmono2.0-cil mono-runtime # Mono 2
#libglu1-mesa libxcursor1 libxrandr2 libc6:i386 libgl1-mesa-glx:i386 libxcursor1:i386 libxrandr2:i386 # 32/64 bit prerequisites for unity3d

INSTANCE_NAME=$1
LAUNCHER="./unturned/RocketLauncher"

if [ ! -f $LAUNCHER ]; then #Well, somebody can't configure bash scripts...
	if [ -f ../../../install_304930.vdf ]; then 
		LAUNCHER="../../../RocketLauncher"
		if [ ! -f $LAUNCHER ]; then
			if [ -f RocketLauncher ]; then
				mv RocketLauncher $LAUNCHER
			fi
		fi
	fi
fi

ulimit -n 2048

if [ -f $LAUNCHER ]; then
	mono $LAUNCHER $UNTURNED_HOME
else
	echo "RocketLauncher not found"
fi


