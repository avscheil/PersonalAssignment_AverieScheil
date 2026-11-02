const LABELS = {
    water_stress: 'Water stress',
    leaf_density: 'Leaf density',
    nitrogen_content: 'Nitrogen content'
};

const map = L.map('map').setView([34.05, -118.24], 13);

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '© OpenStreetMap contributors'
}).addTo(map);

let heatLayer = null;
const cache = new Map();

// ---- TEMPORARY fake data ----
function fakePayload(metric) {
    const pts = [];
    for (let i = 0; i < 300; i++) {
        pts.push([
            34.05 + (Math.random() - 0.5) * 0.04,
            -118.24 + (Math.random() - 0.5) * 0.04,
            Math.random() * 100
        ]);
    }
    return { metric, p5: 5, p95: 95, points: pts };
}

async function loadPayload(metric) {
    if (cache.has(metric)) return cache.get(metric);
    const payload = fakePayload(metric);
    cache.set(metric, payload);
    return payload;
}

function render(payload) {
    const span = (payload.p95 - payload.p5) || 1;

    const data = payload.points.map(([lat, lng, v]) => [
        lat, lng, Math.min(1, Math.max(0, (v - payload.p5) / span))
    ]);

    if (heatLayer) map.removeLayer(heatLayer);

    heatLayer = L.heatLayer(data, {
        radius: 25,
        blur: 18,
        maxZoom: 17,
        max: 1.0,
        gradient: { 0.0: '#2c7bb6', 0.5: '#ffffbf', 1.0: '#d7191c' }
    }).addTo(map);

    drawLegend(payload);
}

function drawLegend(p) {
    document.getElementById('legend').innerHTML = `
        <strong>${LABELS[p.metric] ?? p.metric}</strong>
        <div style="display:flex;align-items:center;gap:8px;max-width:360px;">
            <span>${p.p5.toFixed(2)}</span>
            <div style="flex:1;height:14px;background:linear-gradient(to right,#2c7bb6,#ffffbf,#d7191c);"></div>
            <span>${p.p95.toFixed(2)}</span>
        </div>
        <small>Scale clipped to 5th–95th percentile.</small>`;
}

async function showMetric(metric) {
    render(await loadPayload(metric));
}

function buildToggle(metrics) {
    const el = document.getElementById('metric-toggle');
    el.innerHTML = metrics.map((m, i) => `
        <label style="margin-right:14px;">
            <input type="radio" name="metric" value="${m}" ${i === 0 ? 'checked' : ''}>
            ${LABELS[m] ?? m}
        </label>`).join('');

    el.addEventListener('change', e => showMetric(e.target.value));
    showMetric(metrics[0]);
}

buildToggle(['water_stress', 'leaf_density', 'nitrogen_content']);