docker run -d -p 27017:27017 --name shopping-mongo mongo

docker ps -a

docker logs -f shopping-mongo

docker exec -it shopping-mongo /bin/bash

mongsh

show dbs

use CatalogDb
db.createCollection('Products')

db.Products.insert/insertMany


db.Products.find({}).pretty()


db.Products.remove({})

show collections

docker start container-id

docker-compose -f ./docker-compose.yml -f ./docker-compose.override.yml up -d


docker-compose -f ./docker-compose.yml -f ./docker-compose.override.yml down


docker ps -aq
docker stop $(docker ps -aq)

docker rm $(docker ps -aq)

docker rmi $(docker images -q)

docker system prune

docker images

docker run -d -p 6379:6379 --name aspnetrun-redis redis


docker logs -f aspnetrun-redis

docker exec -it aspnetrun-redis /bin/bash

redis-cli


































