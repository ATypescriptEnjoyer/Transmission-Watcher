# Transmission Watcher

Watches transmission for completed seeds and automatically pauses them.


## How to use

Edit config.json with your transmission hostname, post, and RPC location, then run start.sh

## Notes

* docker-compose.yml contains my network that my transmission is located in, and works internally. feel free to put it in its own network if you're planning on accessing transmission via an external source.
* I haven't implemented password protection because my transmission client isn't public facing, code will require editing slightly if you need username/password implementation, just edit `config.json` and the appropriate model inside the Models folder, then plug it into the transmission object.
* This code works for me, your milage may vary, but I have no doubt that your problems will be due to misconfiguration, please make sure your `config.json` and `docker-compose.yml` match your needs.
* I have the seed ratio check set to 1.3 to ensure positive ratio, and I have a check to see if im the only seeder, and if so to keep seeding until nobody needs the file anymore. Feel free to remove this and edit it in `Transmission.cs`.

Any issues, log an issue and i'll fix it if it's something thats related to the repo.
