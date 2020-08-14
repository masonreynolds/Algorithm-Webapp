const width = 500;
const height = 500;
const offset = 230;
const projection = d3.geoOrthographic();
const path = d3.geoPath().projection(projection);
const center = [width/2 + offset, height/2];

var config = {
    verticalTilt: 0,
    rotation: 0
};

var markerGroup = null;
var positions = [];
var timer = null;
var links = [];
var svg = null;
var count = 0;

function createGlobe() {
    svg = d3.select('#globe').attr('viewBox', [offset, 0, width, height]);
        
    markerGroup = svg.append('g');

    drawGraticule();
    drawGlobe(); 

    document.getElementById('globe').addEventListener('mousedown', function(event) {
        if (event.button == 0)
        {
            if (document.getElementById('globe').onmousemove)
            {
                document.getElementById('globe').onmousemove = null;
                timer.stop();
            }
            
            let start = config.verticalTilt;
            let rot = config.rotation;
            let mouseX = event.clientX;
            let mouseY = event.clientY;
            let dragging = true;
            
            document.getElementById('globe').onmousemove = function(event) {
                config.rotation = rot - (mouseX - event.clientX) * 0.2;
                config.verticalTilt = start + (mouseY - event.clientY) * 0.2;

                if (dragging)
                {
                    enableRotation();
                    dragging = false;
                }
            };
        }
        else if (event.button == 2)
        {
            var m = document.getElementById('globe').getScreenCTM();
            var p = document.getElementById('globe').createSVGPoint(); 
            p.x = event.clientX;
            p.y = event.clientY;
            p = p.matrixTransform(m.inverse());
            
            let coords = projection.invert([p.x, p.y]);
            positions.push({id: Math.floor(Math.random() * 100000), lat: parseFloat(coords[1].toFixed(4)), 
                            lon: parseFloat(coords[0].toFixed(4)), name: 'Custom Location ' + (++count)});
            updateGlobe(null, []);
        }
    });

    document.getElementById('globe').addEventListener('mouseup', function(event) {
        if (event.button == 0)
        {
            document.getElementById('globe').onmousemove = null;

            if (timer)
            {
                timer.stop();
            }
        }
    });

    document.getElementById('globe').addEventListener('ondragstart', function(event) {
        return false;
    });

    document.getElementById('globe').addEventListener('contextmenu', function(event) {
        event.preventDefault();
    }, false);
}

function enableRotation() {
    timer = d3.timer(function () {
        projection.rotate([config.rotation, config.verticalTilt, 0]);
        svg.selectAll('path').attr('d', path);
        updateGlobe();
    });
}   

function drawGlobe() {  
    d3.queue()
        .defer(d3.json, 'https://gist.githubusercontent.com/mbostock/4090846/raw/d534aba169207548a8a3d670c9c2cc719ff05c47/world-110m.json')
        .await((error, worldData) => {
            svg.selectAll('.segment')
                .data(topojson.feature(worldData, worldData.objects.countries).features)
                .enter().append('path')
                .attr('class', 'segment')
                .attr('d', path)
                .style('stroke', 'black')
                .style('stroke-width', '1px')
                .style('fill', (d, i) => 'green')
                .style('opacity', '0.75');               
        });
}

function drawGraticule() {
    const graticule = d3.geoGraticule()
        .step([10, 10]);

    svg.append('path')
        .datum(graticule)
        .attr('class', 'graticule')
        .attr('d', path)
        .style('fill', 'white')
        .style('stroke', 'gray');
}

function drawMarkers() {
    const markers = markerGroup.selectAll('circle')
        .data(positions);
        
    markers
        .enter()
        .append('circle')
        .on('mousedown', function(pose) {
            d3.event.stopPropagation();

            if (d3.event.button == 0)
            {
                document.getElementById('globe').onmousemove = function(event) {
                    var m = document.getElementById('globe').getScreenCTM();
                    var p = document.getElementById('globe').createSVGPoint(); 
                    p.x = event.clientX;
                    p.y = event.clientY;
                    p = p.matrixTransform(m.inverse());
                    let coords = projection.invert([p.x, p.y]);
                    pose.lat = coords[1];
                    pose.lon = coords[0];
                    updateGlobe(null, []);
                };
            }
            else if (d3.event.button == 2)
            {
                positions = positions.filter(p => p.lat != pose.lat && p.lon != pose.lon);
                this.remove();
                updateGlobe(null, []);
            }
        })
        .merge(markers)
        .attr('cx', d => projection([d.lon, d.lat])[0])
        .attr('cy', d => projection([d.lon, d.lat])[1])
        .attr('fill', d => {
            const coordinate = [d.lon, d.lat];
            gdistance = d3.geoDistance(coordinate, projection.invert(center));
            return gdistance > 1.57 ? 'none' : 'red';
        })
        .attr('r', 7);

    markerGroup.each(function () {
        this.parentNode.appendChild(this);
    });
}

function drawArcs() {
    if (links.length > 0)
    {
        let geoPath = d3.geoPath(projection);
        let poses = {
            'type': 'LineString',
            'coordinates': []
        };

        links.forEach(l => { poses.coordinates.push([l.start.lon, l.start.lat]); });
        poses.coordinates.push([links[links.length-1].end.lon, links[links.length-1].end.lat])

        markerGroup.append('path')
            .attr('d', geoPath(poses))
            .attr('fill', 'transparent')
            .attr('stroke-width', 3)
            .attr('stroke', 'black');
    }
}

function updateGlobe(poses, conns) {
    if (poses)
    {
        positions = poses;
    }

    if (conns)
    {
        links = conns;
    }

    DotNet.invokeMethod('SimulatedAnnealingUI', 'UpdatePoses', positions);
    markerGroup.selectAll('circle').remove();
    markerGroup.selectAll('path').remove();
    drawMarkers();
    drawArcs();
}

function addPosition(pose) {
    positions.push(pose);
    updateGlobe();
}

function clearPoses() {
    count = 0;
    updateGlobe([], []);
}

function getPoses() {
    return positions;
}