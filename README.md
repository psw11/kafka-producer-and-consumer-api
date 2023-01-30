That is an example to use the Kafka producer and consumer API. \
To start one you can do some steps below. 

#### Start Kafka cluster
- Build kafka image by running the file ".\cluster\kafka-image\build-image.bat"
- Start or restart Kafka cluster by running the file ".\cluster\restart.bat"
- Wait for start the containers. It takes about 10 seconds

#### Open web UI to see cluster data
- Kafka viewer http://localhost:9095/
- Zookeeper viewer http://localhost:2182/ (connection string "zookeeper:2181")

#### Use examples
- Open project file ".\src\Kafka.csproj" in the VisualStudio

#### Stop Kafka cluster
- Run the file ".\cluster\stop.bat"
- Remove manually folder ".\cluster\volumes\"
