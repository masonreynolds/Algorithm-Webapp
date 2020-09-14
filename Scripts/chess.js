var fieldSize = 90;
var svg = null;

function drawChessBoard(boardDimension) {
    let boardSize = boardDimension*fieldSize;
    let board = [];
    svgs = [];

    for (var i = 0; i < boardDimension*boardDimension; i++) {
        board.push({
            x: i % boardDimension,
            y: Math.floor(i / boardDimension),
            piece: 0
        });
    };

    let row = d3.select("#table").append("div");

    svg = row.append("div")
        .style("width", boardSize + "px")
        .style("height", boardSize + "px")
        .append("svg")
        .attr("width", boardSize + "px")
        .attr("height", boardSize + "px")
        .selectAll(".fields")
        .data(board)
        .enter()
        .append("g");

    svg.append("rect")
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
                return "beige";
            else
                return "tan";
        })
        .style("stroke-width", 1)
        .style("stroke", "black");
}

function updateChess(board) {
    svg.selectAll("text").remove();

    for (let i = 0; i < board.boardSize; i++)
    {
        svg.append("text")
            .style("font-size", fieldSize)
            .attr("text-anchor", "middle")
            .attr("dy", (((board.queens[i]+1) * fieldSize) - (fieldSize/8)) + "px")
            .attr("dx", (((i+1) * fieldSize) - (fieldSize/2)) + "px")
            .text('\u2655');
    }
}