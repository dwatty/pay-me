#!/bin/zsh

helpFunction()
{
   echo "DESCRIPTION: Performs a local deployment or teardown of Redis"
   echo "             to the k8s environment"
   echo ""
   echo "      USAGE: To Deploy: -a up"
   echo "             To Delete: -a down"
   echo ""
   exit 1 # Exit script after printing help
}

while getopts "a:" opt
do
   case "$opt" in
      a ) argAction="$OPTARG" ;;
      ? ) helpFunction ;; # Print helpFunction in case parameter is non-existent
   esac
done

# Print helpFunction in case parameters are empty
if [ -z "$argAction" ]
then
   echo ""
   echo "      ERROR: Some or all of the parameters are empty!";   
   echo ""
   helpFunction
fi

# Begin script in case all parameters are correct
if [[ "$argAction" == "up" ]]; then
    echo ""
    echo "Deploying Redis..."
    kubectl apply -f ../local/redis-deployment.yaml
    kubectl apply -f ../local/redis-service.yaml
    echo "Redis deployed and started!"
    echo "";
else
    echo ""
    echo "Deleting Redis..."
    kubectl delete deployment redis
    kubectl delete service redis
    echo "Redis deleted!"
    echo ""
fi