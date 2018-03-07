docker pull mswarbrick/dockerdemoapi:dev
docker run --net="nat" --rm -it -p 5555:80 mswarbrick/dockerdemoapi:dev

