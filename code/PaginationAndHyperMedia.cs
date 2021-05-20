private object Paginate(string href, int index, int count, int total) {
	dynamic links = new ExpandoObject();
	links.self = new {
		href
	};
	if (index + count < total) {
		links.next = new {
			href = $"{href}?index={index + count}&count={count}"
		};
		links.final = new {
			href = $"{href}?index={total - count}"
		};
	}
	if (index > 0) {
		links.previous = new {
			href = $"{href}?index={index - count}&count={count}"
		};
		links.first = new {
			href = $"{href}?count={count}"
		};
	}
	return links;
}

[HttpGet]
public IActionResult Get(int index, int count = 10) {
	var items = db.ListThings().Skip(index).Take(count);
	var total = db.CountThings();
	var result = new {
		_links = Paginate("/api/things", index, count, total),
		total,
		index,
		count,
		items
	};
	return Ok(result);
}