using System.Collections.Generic;
using Kayak.Http;
using NUnit.Framework;
using Rhino.Mocks;

namespace HttpMock.Unit.Tests
{
	[TestFixture]
	public class EndpointMatchingRuleTests
	{
		[Test]
		public void urls_match_it_returns_true( ) {
			var requestHandler = MockRepository.GenerateStub<IRequestHandler>();
			requestHandler.Path = "test";
			requestHandler.QueryParams = new Dictionary<string, string>();

			var httpRequestHead = new HttpRequestHead { Uri = "test" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}

		[Test]
		public void urls_and_methods_the_same_it_returns_true() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandler>();
			requestHandler.Path = "test";
			requestHandler.Method = "PUT";
			requestHandler.QueryParams = new Dictionary<string, string>();

			var httpRequestHead = new HttpRequestHead { Uri = "test", Method = "PUT" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}

		[Test]
		public void urls_and_methods_differ_it_returns_false() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandler>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string>();
			var httpRequestHead = new HttpRequestHead { Uri = "test", Method = "PUT" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead), Is.False);
		}

		[Test]
		public void urls_differ_and_methods_match_it_returns_false() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandler>();
			requestHandler.Path = "pest";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string>();
			var httpRequestHead = new HttpRequestHead { Uri = "test", Method = "GET" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead), Is.False);
		}

		[Test]
		public void urls_and_methods_match_queryparams_differ_it_returns_false() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandler>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string> { { "myParam", "one" } };

			var httpRequestHead = new HttpRequestHead { Uri = "test", Method = "GET" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead), Is.False);
		}

		[Test]
		public void urls_and_methods_match_and_queryparams_exist_it_returns_true() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandler>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string> { { "myParam", "one" } };

			var httpRequestHead = new HttpRequestHead { Uri = "test?oauth_consumer_key=test-api&elvis=alive&moonlandings=faked&myParam=one", Method = "GET" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}

		[Test]
		public void urls_and_methods_match_and_queryparams_does_not_exist_it_returns_false() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandler>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string> { { "myParam", "one" } };

			var httpRequestHead = new HttpRequestHead { Uri = "test?oauth_consumer_key=test-api&elvis=alive&moonlandings=faked", Method = "GET" };
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead), Is.False);
		}


		[Test]
		public void urls_and_methods_match_and_no_query_params_are_set_but_request_has_query_params_returns_true()
		{
			var requestHandler = MockRepository.GenerateStub<IRequestHandler>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string> ();

			var httpRequestHead = new HttpRequestHead { Uri = "test?oauth_consumer_key=test-api&elvis=alive&moonlandings=faked", Method = "GET" };
			var endpointMatchingRule = new EndpointMatchingRule();

			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead), Is.True);
		}

		[Test]
		public void urls_and_methods_and_queryparams_match_it_returns_true() {
			var requestHandler = MockRepository.GenerateStub<IRequestHandler>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string>{{"myParam", "one"}};

			var httpRequestHead = new HttpRequestHead { Uri = "test?myParam=one", Method = "GET" };
			
			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}

		[Test]
		public void should_do_a_case_insensitive_match_on_query_string_parameter_values() {

			var requestHandler = MockRepository.GenerateStub<IRequestHandler>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string> { { "myParam", "one" } };

			var httpRequestHead = new HttpRequestHead { Uri = "test?myParam=OnE", Method = "GET" };

			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
		}

		[Test]
		public void should_match_when_the_query_string_has_a_trailing_ampersand()
		{

			var requestHandler = MockRepository.GenerateStub<IRequestHandler>();
			requestHandler.Path = "test";
			requestHandler.Method = "GET";
			requestHandler.QueryParams = new Dictionary<string, string> { { "a", "b" } ,{"c","d"}};

			var httpRequestHead = new HttpRequestHead { Uri = "test?a=b&c=d&", Method = "GET" };

			var endpointMatchingRule = new EndpointMatchingRule();
			Assert.That(endpointMatchingRule.IsEndpointMatch(requestHandler, httpRequestHead));
			
		}
	}
}