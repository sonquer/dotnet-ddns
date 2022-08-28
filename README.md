![OPEN_DDNS](https://user-images.githubusercontent.com/10676657/187038251-f64830b7-a3f8-46fc-bba2-0b33b4fc6b49.png)

![license](https://img.shields.io/github/license/sonquer/open-ddns?colorA=192330&colorB=c70039&style=for-the-badge)
![last-commit](https://img.shields.io/github/last-commit/sonquer/open-ddns?colorA=192330&style=for-the-badge)
![repo-size](https://img.shields.io/github/repo-size/sonquer/open-ddns?colorA=192330&style=for-the-badge)
![issues](https://img.shields.io/github/issues-raw/sonquer/open-ddns?colorA=192330&style=for-the-badge)

---

[![Docker image](https://github.com/sonquer/open-ddns/actions/workflows/docker-publish.yml/badge.svg?branch=main)](https://github.com/sonquer/open-ddns/actions/workflows/docker-publish.yml)
[![CodeFactor](https://www.codefactor.io/repository/github/sonquer/open-ddns/badge)](https://www.codefactor.io/repository/github/sonquer/open-ddns)

## Configuration directory & required files
```
.env/
├─ providers.json
├─ database.db
```

**providers.json**

```
{
    "Providers": [
        {
            "Provider": "OvhProvider",
            "Domain": "example.com",
            "SubDomain": "www", 
            "Secret": "<secret>"
        }
    ]
}
```

**database.db** will be generated automatically

---

## docker-compose.yml

```
version: '3.4'

services:
  open-ddns-host:
    image: ghcr.io/sonquer/open-ddns:main
    volumes:
      - ./.env:/config/
    ports:
      - "5005:80"
```

```
docker-compose up -d
```

---

## Application UI

![Screenshot 2022-08-28 145418](https://user-images.githubusercontent.com/10676657/187075109-2d6150c8-7de9-4fc7-b0e6-0e6bf333b205.png)
