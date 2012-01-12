# MVC.NET::Ping
An MVC ActionResult that should indicate the health of your service.


## Usage
Here is a simple example (see `Ping.Driver`)

        public ActionResult Ping()
        {
            return new PingResult(
                new PingOpts
                    {
                        Check = () => false,
                        ErrorText = "aw snap."
                    });
        }

## Options

When building/mounting your application, use the configuration,
specify:

* `Version` is an accessor for your application version. `VERSION`
  would be a good idea.
* `CheckUrl` is a url that `ping` will fetch and run `OkRegex` on. If
  the match is ok, we're good. You must specify `CheckUrl` and
`OkRegex` togather. `TimeoutSecs` is the amount of seconds we wait
until spitting out an error.
* `Check` will accept a block to run. This is a good alternative to
  `CheckUrl`: run a couple of sanity checks to indicate you're good.
* `OkCode`, `ErrorCode`, `OkText`, `ErrorText` are configuration for
  you to use, to configure against LB quirks. The default config should
work against ELBs (Amazon elastic LB).

## Headers

`ping` will output intelligent headers. First `x-ping-error` will try to
explain why ping failed.  

Next, `x-app-version` will expose the current deployed version of your
app. This is good in order to validate nothing crawled up to production,
as well as validation for post-production deployment.  

`ping` will bust any browser/client cache for you.


## Contributing

Albacore is used to handle the build.

        $ rake build
        $ rake zip
        
(see `Rakefile`)


Fork, implement, add tests, pull request, get my everlasting thanks and a respectable place here :).


## Copyright

Copyright (c) 2011 [Dotan Nahum](http://gplus.to/dotan) [@jondot](http://twitter.com/jondot). See MIT-LICENSE for further details.




