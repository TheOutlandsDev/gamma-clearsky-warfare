# OUTLANDS GAMMA local session console

This source-only prototype observes a local game event file. It does not host the game world, store character profiles or enforce multiplayer rules.

With Node.js and npm installed, run `npm ci`, `npm run build`, then `npm test` in this directory. `npm start` reads the expected local event file relative to a separately installed workshop layout. Use `node dist/server.js --events <path> --port <port>` to select a test event file and port. The listener binds to loopback only. Open `http://127.0.0.1:37771/` when using the default port.

`GET /api/status` shows observed events and the intended Loner Story policy; `GET /api/policy` describes that policy; `GET /metrics` exposes text metrics. The solo count is one. A host count is derived from xrRazom when available. A policy match checks observed faction and Story mode; it cannot block a modified client or prove matching game content.

The local integration test uses synthetic events. It does not start GAMMA or verify multiplayer synchronization. Do not publish a runtime event file, which may contain session details.
