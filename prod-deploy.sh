#!/bin/zsh

# Build our contains for FE and BE
docker build --target production -t watterson.azurecr.io/payme-client ./PayMe.Client &&
docker build -t watterson.azurecr.io/payme-server -f PayMe.Server/Dockerfile . &&

# Push our images to our registry
docker login watterson.azurecr.io
docker push watterson.azurecr.io/payme-client
docker push watterson.azurecr.io/payme-server

# Deploy everything to k8s
kubectl apply -f ./PayMe.Infra/prod/redis-deployment.yaml &&
kubectl apply -f ./PayMe.Infra/prod/redis-service.yaml &&
kubectl apply -f ./PayMe.Infra/prod/roles.yaml &&
kubectl apply -f ./PayMe.Infra/prod/backend-deployment.yaml &&
kubectl apply -f ./PayMe.Infra/prod/backend-service.yaml &&
kubectl apply -f ./PayMe.Infra/prod/frontend-deployment.yaml &&
kubectl apply -f ./PayMe.Infra/prod/frontend-service.yaml &&

#kubectl rollout restart deployment/redis &&
kubectl rollout restart deployment/backend &&
kubectl rollout restart deployment/frontend &&

# See what's running
kubectl get services