#!/bin/zsh

# The necessary role
kubectl apply -f ../local/roles.yaml

echo "Deploying Redis..."
./local-redis.sh -a up

echo "Deploying Backend..."
./local-backend.sh -a up

echo "Deploying Frontend..."
./local-frontend.sh -a up