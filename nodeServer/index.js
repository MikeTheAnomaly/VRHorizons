const WebSocket = require('ws');
const os = require('os');
const server = new WebSocket.Server({
    host: "127.0.0.1",
  port: 9515
});
console.log(server.address());

let sockets = [];
server.on('connection', function(socket) {
  sockets.push(socket);
  console.log("connected to client")

  // When you receive a message, send that message to every socket.
  socket.on('message', function(msg) {
    console.log(msg.toString())
    sockets.forEach(s => s.send(msg.toString()));
  });

  // When a socket closes, or disconnects, remove it from the array.
  socket.on('close', function() {
    sockets = sockets.filter(s => s !== socket);
  });
});

const interfaces = os.networkInterfaces();

for (const interfaceName of Object.keys(interfaces)) {
  const addresses = interfaces[interfaceName];
  for (const address of addresses) {
    if (address.family === 'IPv4' && !address.internal) {
      console.log(`Local IP address: ${address.address}`);
    }
  }
}