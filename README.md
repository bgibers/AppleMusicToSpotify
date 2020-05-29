# AppleMusicToSpotify

This project will eventually provide various tools used for enhancing a users spotify library. The initial endpoint created allows a user to fully migrate their apple music library to spotify.
## Generating a developer key

- Follow apples [instructions](https://help.apple.com/developer-account/#/devce5522674) to create a music kit identifer
  - Take note of music ID, and teamID, also download your private key generated.
  
- Use the following js snippet to generate your token

```js
  "use strict";

const fs      = require("fs");
const jwt     = require("jsonwebtoken");

const privateKey = fs.readFileSync("AuthKey.p8").toString(); // replace with key file
const teamId     = "ABCDE12345"; // insert teamId
const keyId      = "ABCDE12345"; // insert musicId

const jwtToken = jwt.sign({}, privateKey, {
  algorithm: "ES256", // change this if you may
  expiresIn: "180d",
  issuer: teamId,
  header: {
    alg: "ES256",
    kid: keyId
  }
});

console.log(jwtToken);
```
