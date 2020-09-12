var fieldSize = 80;
var board = null;
var cells = null;
var svg = null;

function drawSudokuBoard() {
    let boardSize = 9*fieldSize + (fieldSize/2);
    let s = [];
    svgs = [];

    for (var i = 0; i < 9; i++) {
        s.push({
            x: i % 3,
            y: Math.floor(i / 3)
        });
    };

    let row = d3.select("#table");

    svg = row.append("div")
        .style("width", boardSize + "px")
        .style("height", boardSize + "px")
        .append("svg")
        .attr("width", boardSize + "px")
        .attr("height", boardSize + "px");

    let sections = svg.selectAll("section")
        .data(s)
        .enter()
        .append("g")
        .attr("transform", function (d) {
            return "translate(" + (d.x*3*fieldSize + (d.x*fieldSize/4)) + ", " + (d.y*3*fieldSize + (d.y*fieldSize/4)) + ")";
        });

    cells = sections.selectAll("cells")
            .data(s)
            .enter()
            .append("g");

    cells.append("rect")
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
        .style("fill", "rgb(32, 32, 32)")
        .style("stroke-width", 1)
        .style("stroke", "white");

    svg.on('contextmenu', function() {
            d3.event.preventDefault();
        });
}

function updateSudokuBoard(b) {
    cells.selectAll("text").remove();
    values = [];
    board = b;

    for (let sr = 0; sr < 9; sr+=3)
    {
        for (let sc = 0; sc < 9; sc+=3)
        {
            var sRow = (sr / 3) * 3;
            var sCol = (sc / 3) * 3;

            for (var i = sRow; i < sRow+3; i++)
            {
                for (var k = sCol; k < sCol+3; k++)
                {
                    values.push(board[i][k].value);
                }
            }
        }   
    }

    cells.append("text")
        .attr("x", function (d) {
            return d.x*fieldSize + (fieldSize/2);
        })
        .attr("y", function (d) {
            return d.y*fieldSize + (3*fieldSize/4);
        })
        .attr("text-anchor", "middle")
        .style("font-size", 50)
        .style("fill", "white");

    var texts = d3.selectAll('text')
                .data(values);
                
    texts.enter()

    texts.text(function(d) {
        if (d == 0)
        {
            return "";
        }
        else
        {
		    return d;
        }
	});
}