#!/bin/zsh

helpFunction()
{
   echo "DESCRIPTION: Performs a local deployment or teardown of the server"
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
    echo "Deploying Backend Environment..."
    kubectl apply -f ../local/backend-deployment.yaml
    kubectl apply -f ../local/backend-service.yaml
    echo "Backend deployed and started!"
    echo "";
else
    echo ""
    echo "Deleting Backend Environment..."
    kubectl delete deployment backend
    kubectl delete service paymeapp
    echo "Backend deleted!"
    echo ""
fi