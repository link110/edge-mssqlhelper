var edge = require('../lib/edge');

var Invoke = edge.func('mssqlhelper.dll');

Invoke({
	command:"select * from 系统用户",
	et:"ExecuteType.Query"
}, function (error, result) {
	if (error) throw error;
	console.log(result['Result']);
});