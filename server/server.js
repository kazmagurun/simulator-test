const
    io = require("socket.io"),
    server = io.listen(8080);

//let
//    sequenceNumberByClient = new Map();
console.info("starting");
// event fired every time a new client connects:
server.on("connection", (socket) => {
    console.info(`Client connected [id=${socket.id}]`);
    // initialize this client's sequence number
//    sequenceNumberByClient.set(socket, 1);

    // when socket disconnects, remove it from the list:
    socket.on("disconnect", () => {
//        sequenceNumberByClient.delete(socket);
        console.info(`Client gone [id=${socket.id}]`);
    });

    socket.on("telemetry", (data) => {
        steering_angle = data["steering_angle"];
        throttle = data["throttle"];
        speed = data["speed"];
        image = data["image"];
        console.info("received telemetry data. angle = " + steering_angle
                    + "speed = " + speed
                    + "throttle = " + throttle
        );
    });

   // socket.emit("telemetry", "");
});

// sends each client its current sequence number
//setInterval(() => {
//    for (const [client, sequenceNumber] of sequenceNumberByClient.entries()) {
//        client.emit("seq-num", sequenceNumber);
//        sequenceNumberByClient.set(client, sequenceNumber + 1);
//    }
//}, 1000);
