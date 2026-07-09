extends Node
class_name NetworkManager


var http := HTTPRequest.new()



func connect_server():

	add_child(http)


	http.request_completed.connect(
		_on_response
	)


	http.request(
		"http://localhost:5107/api/ping"
	)



func _on_response(
	result,
	response_code,
	headers,
	body
):

	var text = body.get_string_from_utf8()

	Logger.info(text)
