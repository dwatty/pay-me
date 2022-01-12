#!/bin/zsh

# Build our contains for FE and BE
docker build --target production -t payme-client:dev ./PayMe.Client &&
docker build -t payme-server:dev -f PayMe.Server/Dockerfile . &&

# Deploy everything to k8s
#kubectl apply -f ./PayMe.Infra/local/redis-deployment.yaml &&
#kubectl apply -f ./PayMe.Infra/local/redis-service.yaml &&
kubectl apply -f ./PayMe.Infra/local/roles.yaml &&
kubectl apply -f ./PayMe.Infra/local/backend-deployment.yaml &&
kubectl apply -f ./PayMe.Infra/local/backend-service.yaml &&
kubectl apply -f ./PayMe.Infra/local/frontend-deployment.yaml &&
kubectl apply -f ./PayMe.Infra/local/frontend-service.yaml &&

#kubectl rollout restart deployment/redis &&
kubectl rollout restart deployment/backend &&
kubectl rollout restart deployment/frontend &&

# See what's running
kubectl get services