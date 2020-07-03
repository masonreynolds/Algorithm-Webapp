const width = 500;
const height = 500;
const offset = 230;
const projection = d3.geoOrthographic();
const path = d3.geoPath().projection(projection);
const center = [width/2 + offset, height/2];

let config = {
    speed: 0.0,
    verticalTilt: 0,
    horizontalTilt: 0,
    rotation: 0
};

let markerGroup = null;
let rendered = false;
let locations = [];
let links = [];
let timer = null;
let svg = null;

function createGlobe(locs, conns) {
    if (!rendered)
    {
        let dragging = false;
        rendered = true;
        let mouseX = 0;
        let mouseY = 0;
        let start = 0;
        let rot = 0;

        svg = d3.select('#globe').attr('viewBox', [offset, 0, width, height]);
            
        markerGroup = svg.append('g');

        drawGraticule();
        drawGlobe(); 

        function drawGlobe() {  
            d3.queue()
                .defer(d3.json, 'https://gist.githubusercontent.com/mbostock/4090846/raw/d534aba169207548a8a3d670c9c2cc719ff05c47/world-110m.json')
                .await((error, worldData) => {
                    svg.selectAll(".segment")
                        .data(topojson.feature(worldData, worldData.objects.countries).features)
                        .enter().append("path")
                        .attr("class", "segment")
                        .attr("d", path)
                        .style("stroke", "black")
                        .style("stroke-width", "1px")
                        .style("fill", (d, i) => 'green')
                        .style("opacity", "0.75");               
                });
        }

        function drawGraticule() {
            const graticule = d3.geoGraticule()
                .step([10, 10]);

            svg.append("path")
                .datum(graticule)
                .attr("class", "graticule")
                .attr("d", path)
                .style("fill", "white")
                .style("stroke", "gray");
        }
    
        function onMouseMove(event) {
            config.rotation = rot - (mouseX - event.clientX) * 0.2;
            config.verticalTilt = start + (mouseY - event.clientY) * 0.2;

            if (dragging)
            {
                enableRotation();
                dragging = false;
            }
        }

        document.getElementById('globe').addEventListener("mousedown", function(event) {
            if (timer)
            {
                document.getElementById('globe').removeEventListener('mousemove', onMouseMove);
                timer.stop();
            }

            start = config.verticalTilt;
            rot = config.rotation;
            mouseX = event.clientX;
            mouseY = event.clientY;
            dragging = true;
            
            document.getElementById('globe').addEventListener('mousemove', onMouseMove);
        });

        document.getElementById('globe').addEventListener("mouseup", function() {
            document.getElementById('globe').removeEventListener('mousemove', onMouseMove);
            timer.stop();
        });
    
        document.getElementById('globe').addEventListener("ondragstart", function(event) {
            return false;
        });
    }
    else
    {
        markerGroup.selectAll('circle').remove();
        markerGroup.selectAll('path').remove();
        locations = locs;
        links = conns;
        drawMarkers();
        drawArcs();
    }
}

function setRendered(isRendered) {
    rendered = isRendered;
}

function enableRotation() {
    timer = d3.timer(function (elapsed) {
        projection.rotate([config.rotation, config.verticalTilt, config.horizontalTilt]);
        svg.selectAll("path").attr("d", path);
        drawMarkers();
        drawArcs();
    });
}   

function drawMarkers() {
    const markers = markerGroup.selectAll('circle')
        .data(locations);
        
    markers
        .enter()
        .append('circle')
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
            "type": "LineString",
            "coordinates": []
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