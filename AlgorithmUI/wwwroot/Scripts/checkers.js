var fieldSize = 90;
var board = null;
var svg = null;

function drawCheckersBoard() {
    let boardSize = 8*fieldSize;
    let b = [];
    svgs = [];

    for (var i = 0; i < 64; i++) {
        b.push({
            x: i % 8,
            y: Math.floor(i / 8)
        });
    };

    let row = d3.select("#table").append("div");

    svg = row.append("div")
        .style("width", boardSize + "px")
        .style("height", boardSize + "px")
        .append("svg")
        .attr("width", boardSize + "px")
        .attr("height", boardSize + "px");

    svg.selectAll(".fields")
        .data(b)
        .enter()
        .append("rect")
        .style("class", "fields")
        .style("class", "rects")
        .attr("x", function (d) {
            return d.x*fieldSize;
        })
        .attr("y", function (d) {
            return d.y*fieldSize;
        })
        .attr("width", fieldSize + "px")
        .attr("height", fieldSize + "px")
        .style("fill", function (d) {
            if (    ((d.x%2 == 0) && (d.y%2 == 0)) ||
                    ((d.x%2 == 1) && (d.y%2 == 1))  ) 
                return "red";
            else
                return "black";
        })
        .style("stroke-width", 1)
        .style("stroke", "black");

    svg.on('contextmenu', function() {
            d3.event.preventDefault();
        });
}

function updateCheckers(b) {
    svg.selectAll("circle").remove();
    board = b;

    let blacks = svg.selectAll("black")
        .data(board.blackPoses)
        .enter()
        .append("g")
        .attr("y", d => (((d[0]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("x", d => (((d[1]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("width", (fieldSize/2 - 10) + "px")
        .attr("height", (fieldSize/2 - 10) + "px")
        .on('mousedown', function(pose) {
            if (d3.event.button == 2)
            {
                d3.event.preventDefault();
                board.blackPoses = board.blackPoses.filter(b => b[0] != pose[0] || b[1] != pose[1]);
                board.blackKings = board.blackKings.filter(b => b[0] != pose[0] || b[1] != pose[1]);
                DotNet.invokeMethod('SimulatedAnnealingUI', 'updateCheckers', board);
                d3.event.target.remove();
            }
        });
        
    blacks.append("circle")
        .attr("cy", d => (((d[0]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("cx", d => (((d[1]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("r", (fieldSize/2 - 10) + "px")
        .attr("fill", "black")
        .attr("stroke", "white")
        .attr("stroke-width", 3);

    let reds = svg.selectAll("red")
        .data(board.redPoses)
        .enter()
        .append("g")
        .attr("y", d => (((d[0]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("x", d => (((d[1]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("width", (fieldSize/2 - 10) + "px")
        .attr("height", (fieldSize/2 - 10) + "px")
        .on('mousedown', function(pose) {
            if (d3.event.button == 0)
            {
                this.onmousemove = function(event) {
                    let CTM = event.target.getScreenCTM();
                    event.target.setAttributeNS(null, "cx", (event.clientX - CTM.e) / CTM.a);
                    event.target.setAttributeNS(null, "cy", (event.clientY - CTM.f) / CTM.d);
                };

                this.onmouseup = function(event) {
                    let CTM = event.target.getScreenCTM();
                    pose[0] = Math.round(((event.clientY - CTM.f) / CTM.d - fieldSize / 2) / (fieldSize));
                    pose[1] = Math.round(((event.clientX - CTM.e) / CTM.a - fieldSize / 2) / (fieldSize));
                    event.target.setAttributeNS(null, "cx", pose[1] * fieldSize + (fieldSize/2));
                    event.target.setAttributeNS(null, "cy", pose[0] * fieldSize + (fieldSize/2));

                    if (pose[0] == 0 && board.redPoses.includes(pose))
                    {
                        board.redPoses = board.redPoses.filter(r => r[0] != pose[0] || r[1] != pose[1]);
                        board.redKings.push(pose);

                        event.target.setAttributeNS(null, "stroke", "gold");
                    }

                    this.onmousemove = null;
                    DotNet.invokeMethod('SimulatedAnnealingUI', 'updateCheckers', board);
                };
            }
        });
        
    reds.append("circle")
        .attr("cy", d => (((d[0]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("cx", d => (((d[1]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("r", (fieldSize/2 - 10) + "px")
        .attr("fill", "red")
        .attr("stroke", "white")
        .attr("stroke-width", 3);

    let blackKings = svg.selectAll("blackKing")
        .data(board.blackKings)
        .enter()
        .append("g")
        .attr("y", d => (((d[0]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("x", d => (((d[1]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("width", (fieldSize/2 - 10) + "px")
        .attr("height", (fieldSize/2 - 10) + "px")
        .on('mousedown', function(pose) {
            if (d3.event.button == 2)
            {
                d3.event.preventDefault();
                board.blackPoses = board.blackPoses.filter(b => b[0] != pose[0] || b[1] != pose[1]);
                board.blackKings = board.blackKings.filter(b => b[0] != pose[0] || b[1] != pose[1]);
                DotNet.invokeMethod('SimulatedAnnealingUI', 'updateCheckers', board);
                d3.event.target.remove();
            }
        });
        
    blackKings.append("circle")
        .attr("cy", d => (((d[0]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("cx", d => (((d[1]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("r", (fieldSize/2 - 10) + "px")
        .attr("fill", "black")
        .attr("stroke", "gold")
        .attr("stroke-width", 3);

    let redKings = svg.selectAll("redKing")
        .data(board.redKings)
        .enter()
        .append("g")
        .attr("y", d => (((d[0]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("x", d => (((d[1]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("width", (fieldSize/2 - 10) + "px")
        .attr("height", (fieldSize/2 - 10) + "px")
        .on('mousedown', function(pose) {
            if (d3.event.button == 0)
            {
                this.onmousemove = function(event) {
                    let CTM = event.target.getScreenCTM();
                    event.target.setAttributeNS(null, "cx", (event.clientX - CTM.e) / CTM.a);
                    event.target.setAttributeNS(null, "cy", (event.clientY - CTM.f) / CTM.d);
                };

                this.onmouseup = function(event) {
                    let CTM = event.target.getScreenCTM();
                    pose[0] = Math.round(((event.clientY - CTM.f) / CTM.d - fieldSize / 2) / (fieldSize));
                    pose[1] = Math.round(((event.clientX - CTM.e) / CTM.a - fieldSize / 2) / (fieldSize));
                    event.target.setAttributeNS(null, "cx", pose[1] * fieldSize + (fieldSize/2));
                    event.target.setAttributeNS(null, "cy", pose[0] * fieldSize + (fieldSize/2));

                    this.onmousemove = null;
                    DotNet.invokeMethod('SimulatedAnnealingUI', 'updateCheckers', board);
                };
            }
        });
        
    redKings.append("circle")
        .attr("cy", d => (((d[0]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("cx", d => (((d[1]+1) * fieldSize) - (fieldSize/2)) + "px")
        .attr("r", (fieldSize/2 - 10) + "px")
        .attr("fill", "red")
        .attr("stroke", "gold")
        .attr("stroke-width", 3);
}