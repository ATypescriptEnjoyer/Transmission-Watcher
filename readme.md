# Transmission Watcher

Watches transmission for completed seeds and automatically pauses them.


## How to use

```bash
#Example Docker Compose file
version: '3.1'
services:
  transmission-watcher:
    image: ghcr.io/sasharyder/transmission-watcher
    environment:
      - TRANSMISSION_PATH=transmission:9091/transmission/rpc
      - STOP_RATIO=1.3
      - HELP_SOLO_PEERS=true
    container_name: transmission-watcher
    restart: unless-stopped
```

## Dependencies

* .NET 5.0
* Docker

## Notes
* This code works for me, your milage may vary, but I have no doubt that your problems will be due to misconfiguration, please make sure your `docker-compose.yml` matches your needs.

Any issues, log an issue and i'll fix it if it's something thats related to the repo.
