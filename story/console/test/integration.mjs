import assert from 'node:assert/strict';
import { spawn } from 'node:child_process';
import { mkdtemp, appendFile, rm } from 'node:fs/promises';
import { createServer } from 'node:net';
import { tmpdir } from 'node:os';
import { join, resolve } from 'node:path';

const temp = await mkdtemp(join(tmpdir(), 'outlands-gamma-test-'));
const events = join(temp, 'events.jsonl');
const probe = createServer();
await new Promise((ok, fail) => probe.once('error', fail).listen(0, '127.0.0.1', ok));
const address = probe.address();
assert(address && typeof address !== 'string');
const port = address.port;
await new Promise(ok => probe.close(ok));

const child = spawn(process.execPath, [resolve('dist/server.js'), '--events', events, '--port', String(port)], {
  cwd: process.cwd(), stdio: ['ignore', 'pipe', 'pipe']
});
const base = `http://127.0.0.1:${port}`;
async function waitFor(check) {
  for (let attempt = 0; attempt < 100; attempt++) {
    try { const value = await check(); if (value) return value; } catch { /* startup */ }
    await new Promise(ok => setTimeout(ok, 50));
  }
  throw new Error('Timed out waiting for local server state');
}

try {
  await waitFor(async () => (await fetch(`${base}/api/status`)).ok);
  let status = await (await fetch(`${base}/api/status`)).json();
  assert.equal(status.world_loaded, false);
  assert.equal(status.connected_players, null);

  await appendFile(events, '{"version":1,"kind":"world_loaded","unix_time":1793836800,"role":"solo","faction":"stalker","story_mode":true}\n');
  status = await waitFor(async () => {
    const value = await (await fetch(`${base}/api/status`)).json();
    return value.events_seen === 1 ? value : null;
  });
  assert.equal(status.world_loaded, true);
  assert.equal(status.role, 'solo');
  assert.equal(status.policy_match, true);
  assert.equal(status.policy.faction, 'stalker');

  await appendFile(events, '{"version":1,"kind":"world_role","unix_time":1793836810,"role":"host","faction":"stalker","story_mode":true,"peer_count":2}\n');
  status = await waitFor(async () => {
    const value = await (await fetch(`${base}/api/status`)).json();
    return value.events_seen === 2 ? value : null;
  });
  assert.equal(status.role, 'host');
  assert.equal(status.connected_players, 3);
  await appendFile(events, '{"version":1,"kind":"world_heartbeat","unix_time":1793836820,"role":"host","faction":"stalker","story_mode":true,"peer_count":2}\n');
  status = await waitFor(async () => {
    const value = await (await fetch(`${base}/api/status`)).json();
    return value.events_seen === 3 ? value : null;
  });
  assert.equal(status.world_loaded, true);
  assert.equal(status.last_event.kind, 'world_heartbeat');
  assert.ok(status.last_game_event_at);
  assert.equal(status.policy_match, true);
  const serverPolicy = await (await fetch(`${base}/api/policy`)).json();
  assert.equal(serverPolicy.max_players_target, 5);
  const metrics = await (await fetch(`${base}/metrics`)).text();
  assert.match(metrics, /outlands_gamma_events_total 3/);
  assert.match(metrics, /outlands_gamma_policy_match 1/);
  assert.match(metrics, /outlands_gamma_connected_players 3/);
  await appendFile(events, '{"version":1,"kind":"world_role","unix_time":1793836830,"role":"host","faction":"freedom","story_mode":true,"peer_count":2}\n');
  status = await waitFor(async () => {
    const value = await (await fetch(`${base}/api/status`)).json();
    return value.events_seen === 4 ? value : null;
  });
  assert.equal(status.policy_match, false);
  const home = await (await fetch(base)).text();
  assert.match(home, /OUTLANDS/);
  console.log('Local event ingestion, status API, metrics, and branded page passed.');
} finally {
  child.kill('SIGTERM');
  await rm(temp, { recursive: true, force: true });
}
