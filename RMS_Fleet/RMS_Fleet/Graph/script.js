//// Function to create random data in format: [date, amount]

function createData(num,tKM,expected) {
  let data = [];
  let cat=['BB','dd'];
  for (var i = 0; i < num; i++) {
     
    const randomNum = [expected,tKM];
    let d = new Date();
    d.setDate(d.getDate() - i * 30);
	// alert(d);
	// alert(cat[i]);
    data.push({
      date: cat[i],
      amount: randomNum[i] });
  }

  return data;
}

//// Create + Format data
//let data = createData(2,1.97).sort(function (a, b) {return a.date - b.date;});

//// what are these and are they things that someone should edit
//const margin = { top: 30, right: 20, bottom: 30, left: 65 };
//const width = 250 - (margin.left + margin.right);
//const height = 220 - (margin.top + margin.bottom);
//const labelOffset = 50;
//const axisOffset = 16;

//// Set Time Format (JAN, FEB, etc..)
//const timeFormat = d3.timeFormat('%b');

//// Set the scales
//const x = d3.scaleBand().

//rangeRound([0, width]).
//domain(data.map(d => d.date)).
//padding(0.5);
//// alert(x);
//const y = d3.scaleLinear().
//range([height, 0]).
//domain([0, d3.max(data, d => d.amount)]);

//// // Set the axes
//const xAxis = d3.axisBottom().
//scale(x).
//tickSize(0).
//tickFormat(timeFormat);

//const yAxis = d3.axisLeft().
//ticks(4).
//tickSize(-width).
//scale(y.nice());

//// // Set up SVG with initial transform to avoid repeat positioning
//const svg = d3.select('svg').
//attr('class', 'graph').
//attr('width', width + (margin.left + margin.right)).
//attr('height', height + (margin.top + margin.bottom)).
//append('g').
//attr('class', 'group-container').
//attr('transform', `translate(${margin.left}, ${margin.top})`).
//attr('font-family', 'ibm-plex-sans');

//// // Add Y axis
//svg.append('g').
//attr('class', 'axis y').
//attr('stroke-dasharray', '4').
//call(yAxis).
//selectAll('text').
//attr("x", -axisOffset).
//attr('font-family', 'ibm-plex-sans');

//// // Add Y axis label
//const yLabel = svg.select('.y').
//append('text').
//text('Expense (₹)').
//attr('class', 'label').
//attr('transform', `translate(${-labelOffset}, ${height / 2}) rotate(-90)`).
//attr('font-family', 'ibm-plex-sans');

//// // Add X axis
//svg.append('g').
//attr('class', 'axis x').
//attr('transform', `translate(0, ${height})`).
//call(xAxis).
//selectAll('text').
//attr("y", axisOffset).
//attr('font-family', 'ibm-plex-sans');

//// // Add X axis label
//const xLabel = svg.select('.x').
//append('text').
//text('').
//attr('class', 'label').
//attr('transform', `translate(${width / 2}, ${labelOffset})`).
//attr('font-family', 'ibm-plex-sans');

//svg.append('g').
//attr('class', 'bar-container').
//selectAll('rect').
//data(data).
//enter().append('rect').
//attr('class', 'bar').
//attr('x', d => x(d.date)).
//attr('y', d => height).
//attr('height', 0).
//attr('width', x.bandwidth()).
//attr('fill', '#00A78F').
//transition().
//duration(500).
//delay((d, i) => i * 50).
//attr('height', d => height - y(d.amount)).
//attr('y', d => y(d.amount));

//// Select Tooltip
//const tooltip = d3.select('.tooltip');

//const bars = svg.selectAll('.bar').
//on('mouseover', function (d) {
//  let color = d3.color('#00A78F').darker();
//  d3.select(this).
//  attr('fill', color);
//  tooltip.
//  style('display', 'inherit').
//  text(`₹${d.amount}`).
//  style('top', `${y(d.amount) - axisOffset}px`);

//  let bandwidth = x.bandwidth();
//  let tooltipWidth = tooltip.nodes()[0].getBoundingClientRect().width;
//  let offset = (tooltipWidth - bandwidth) / 2;

//  tooltip.
//  style('left', `${x(d.date) + margin.left - offset}px`);
//}).
//on('mouseout', function (d) {
//  d3.select(this).
//  transition().
//  duration(250).
//  attr('fill', '#00A78F');
//  tooltip.
//  style('display', 'none');
//});





function ClearGraph()
{
    // Create + Format data
    let data = createData(0,0).sort(function (a, b) {return a.date - b.date;});

    // what are these and are they things that someone should edit
    const margin = { top: 30, right: 20, bottom: 30, left: 65 };
    const width = 250 - (margin.left + margin.right);
    const height = 220 - (margin.top + margin.bottom);
    const labelOffset = 50;
    const axisOffset = 16;

    // Set Time Format (JAN, FEB, etc..)
    const timeFormat = d3.timeFormat('%b');

    // Set the scales
    const x = d3.scaleBand().

    rangeRound([0, width]).
    domain(data.map(d => d.date)).
    padding(0.5);
    // alert(x);
    const y = d3.scaleLinear().
    range([height, 0]).
    domain([0, d3.max(data, d => d.amount)]);

    // // Set the axes
    const xAxis = d3.axisBottom().
    scale(x).
    tickSize(0).
    tickFormat(timeFormat);

    const yAxis = d3.axisLeft().
    ticks(4).
    tickSize(-width).
    scale(y.nice());

    // // Set up SVG with initial transform to avoid repeat positioning
    const svg = d3.select('svg').
    attr('class', 'graph').
    attr('width', width + (margin.left + margin.right)).
    attr('height', height + (margin.top + margin.bottom)).
    append('g').
    attr('class', 'group-container').
    attr('transform', `translate(${margin.left}, ${margin.top})`).
    attr('font-family', 'ibm-plex-sans');

    // // Add Y axis
    svg.append('g').
    attr('class', 'axis y').
    attr('stroke-dasharray', '4').
    call(yAxis).
    selectAll('text').
    attr("x", -axisOffset).
    attr('font-family', 'ibm-plex-sans');

    // // Add Y axis label
    const yLabel = svg.select('.y').
    append('text').
    text('Expense (₹)').
    attr('class', 'label').
    attr('transform', `translate(${-labelOffset}, ${height / 2}) rotate(-90)`).
    attr('font-family', 'ibm-plex-sans');

    // // Add X axis
    svg.append('g').
    attr('class', 'axis x').
    attr('transform', `translate(0, ${height})`).
    call(xAxis).
    selectAll('text').
    attr("y", axisOffset).
    attr('font-family', 'ibm-plex-sans');

    // // Add X axis label
    const xLabel = svg.select('.x').
    append('text').
    text('').
    attr('class', 'label').
    attr('transform', `translate(${width / 2}, ${labelOffset})`).
    attr('font-family', 'ibm-plex-sans');

    svg.append('g').
    attr('class', 'bar-container').
    selectAll('rect').
    data(data).
    enter().append('rect').
    attr('class', 'bar').
    attr('x', d => x(d.date)).
    attr('y', d => height).
    attr('height', 0).
    attr('width', x.bandwidth()).
    attr('fill', '#00A78F').
    transition().
    duration(500).
    delay((d, i) => i * 50).
    attr('height', d => height - y(d.amount)).
    attr('y', d => y(d.amount));

    // Select Tooltip
    const tooltip = d3.select('.tooltip');

    const bars = svg.selectAll('.bar').
    on('mouseover', function (d) {
      let color = d3.color('#00A78F').darker();
      d3.select(this).
      attr('fill', color);
      tooltip.
      style('display', 'inherit').
      text(`₹${d.amount}`).
      style('top', `${y(d.amount) - axisOffset}px`);

      let bandwidth = x.bandwidth();
      let tooltipWidth = tooltip.nodes()[0].getBoundingClientRect().width;
      let offset = (tooltipWidth - bandwidth) / 2;

      tooltip.
      style('left', `${x(d.date) + margin.left - offset}px`);
    }).
    on('mouseout', function (d) {
      d3.select(this).
      transition().
      duration(250).
      attr('fill', '#00A78F');
      tooltip.
      style('display', 'none');
    });


}