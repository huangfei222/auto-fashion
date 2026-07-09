extends Node


var network


func _ready():

	Logger.info(
		"Client Boot Starting"
	)

	initialize_core()



func initialize_core():

	Logger.info(
		"Core Initialized"
	)


	network = NetworkManager.new()

	add_child(network)


	network.connect_server()


	Logger.info(
		"Network Started"
	)
