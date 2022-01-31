#!/bin/zsh

# Build the Client image
docker build --target production -t payme-client:latest ./PayMe.Client


# docker build -t paymeapp -f PayMe.Server/Dockerfile . &&
# kubectl apply -f ./local-deployment.yaml &&
# kubectl rollout restart deployment/paymeapp &&
# kubectl get services