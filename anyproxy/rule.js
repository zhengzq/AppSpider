
var interceptors = require('./interceptors')

module.exports = {
  summary: 'a rule to hack response',
  *beforeSendResponse (requestDetail, responseDetail) {
    var newResponse = null;

    Object.entries(interceptors).forEach(([key, interceptor]) => {
      if (interceptor && typeof (interceptor) === 'object') {
        if (typeof (interceptor.beforeSendResponse) === 'function') {
          try {
            newResponse = interceptor.beforeSendResponse(requestDetail, responseDetail)
          } catch (error) {
            console.log(['interceptor', key, error])
          }
        }
      }
    });

    if (newResponse) {
      return {
        response: newResponse
      }
    } else {
      return null
    }
  }
};
