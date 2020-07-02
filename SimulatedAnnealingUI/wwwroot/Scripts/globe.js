let rendered = false;

let config = {
    speed: 0.0,
    verticalTilt: 0,
    horizontalTilt: 0,
    rotation: 0
};

const width = 1000;
const height = 750;
const projection = d3.geoOrthographic();
const path = d3.geoPath().projection(projection);
const center = [width/2, height/2];
let markerGroup = null;
let svg = null;
let timer = null;

function createGlobe(locations) {
    if (timer)
    {
        timer.stop();
    }

    if (!rendered)
    {
        rendered=true;

        svg = d3.select('#globe')
            .attr('width', width).attr('height', height)
            .attr('viewBox', [85, -50, width/4*3, height/4*3]);
            
        markerGroup = svg.append('g');

        drawGraticule();
        drawGlobe();    
        enableRotation();    

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
    }
    else
    {
        markerGroup.selectAll('circle').remove();
        enableRotation();
    }

    function enableRotation() {
        timer = d3.timer(function (elapsed) {
            projection.rotate([config.rotation, config.verticalTilt, config.horizontalTilt]);
            svg.selectAll("path").attr("d", path);
            drawMarkers();
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
    

    document.getElementById('globe').addEventListener("mousedown", function(event) {
        document.getElementById('globe').removeEventListener('mousemove', onMouseMove);
        let start = config.verticalTilt;
        let rot = config.rotation;
        let mouseX = event.clientX;
        let mouseY = event.clientY;

        function onMouseMove(event) {
            config.rotation = rot - (mouseX - event.clientX) * 0.1;
            config.verticalTilt = start + (mouseY - event.clientY) * 0.1;
        }
        
        document.getElementById('globe').addEventListener('mousemove', onMouseMove);
        
        document.getElementById('globe').addEventListener("mouseup", function() {
            document.getElementById('globe').removeEventListener('mousemove', onMouseMove);
        });
    });

    document.getElementById('globe').addEventListener("ondragstart", function(event) {
        return false;
    });
}

function setRendered(isRendered) {
    rendered = isRendered;
}