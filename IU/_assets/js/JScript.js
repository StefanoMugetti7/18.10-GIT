(function($) {
  //  add our glowbuttons function.  it accepts
  //  a single parameter that specifies any parameters
  //  our plugin supports
  $.fn.glowbuttons = function(options) {
	
	// build main options before element iteration
	var opts = $.extend({}, $.fn.glowbuttons.defaults, options);
	
	return this.each(function() {
	    var button = $(this);
        //  inject the parent nodes            
        button.wrap('<span class="glow-button"><span class="inner"></span></span>');
        
        //  ** do some browser specific style workarounds
        
        button.parent().each(function(){ 
            var innerWrapper = $(this);
            
            //  ** do some browser specific style workarounds
            
            innerWrapper.parent().each(function(){
            
                //  ** do some browser specific style workarounds
            
            })            
            //  add a class to the outer most node - this helps with theming
            .addClass(o.className)
            //  finally attach to the hover events to run the animation
            .hover(
                function(){
                    $(this).stop();
                    $(this).animate({ backgroundColor: o.to }, o.speed);
                },
                function(){
                    $(this).stop();
                    $(this).animate({ backgroundColor: o.from }, o.speed);
                }
            );                     
        });
	});
  };
  
  // default options - these are used of none others are specified
  $.fn.glowbuttons.defaults = {
	from: '#016bbd',
	to: '#b1ddff',
	className: 'blue',
	speed: 1000
  };

//  invoke the function we just created passing it
//  the jQuery object
})(jQuery);