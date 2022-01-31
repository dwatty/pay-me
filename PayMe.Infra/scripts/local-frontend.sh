#!/bin/zsh

helpFunction()
{
   echo "DESCRIPTION: Performs a local deployment or teardown of the client"
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
    echo "Deploying Frontend Environment..."
    kubectl apply -f ../local/frontend-deployment.yaml
    kubectl apply -f ../local/frontend-service.yaml
    nodeport=$(kubectl get service frontend -o jsonpath="{.spec.ports[0].nodePort}")    
    echo "";
    echo "Frontend deployed and started!"
    echo "Client be accessed at: http://localhost:$nodeport"
    echo "";
else
    echo ""
    echo "Deleting Frontend Environment..."
    kubectl delete deployment frontend
    kubectl delete service frontend
    echo "Frontend deleted!"
    echo ""
fi