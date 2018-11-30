/* ===================================================
 * bootstrap-transition.js v2.0.1
 * http://twitter.github.com/bootstrap/javascript.html#transitions
 * ===================================================
 * Copyright 2012 Twitter, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ========================================================== */

!function( $ ) {

  $(function () {

    "use strict"

    /* CSS TRANSITION SUPPORT (https://gist.github.com/373874)
     * ======================================================= */

    $.support.transition = (function () {
      var thisBody = document.body || document.documentElement
        , thisStyle = thisBody.style
        , support = thisStyle.transition !== undefined || thisStyle.WebkitTransition !== undefined || thisStyle.MozTransition !== undefined || thisStyle.MsTransition !== undefined || thisStyle.OTransition !== undefined

      return support && {
        end: (function () {
          var transitionEnd = "TransitionEnd"
          if ( $.browser.webkit ) {
          	transitionEnd = "webkitTransitionEnd"
          } else if ( $.browser.mozilla ) {
          	transitionEnd = "transitionend"
          } else if ( $.browser.opera ) {
          	transitionEnd = "oTransitionEnd"
          }
          return transitionEnd
        }())
      }
    })()

  })

}( window.jQuery );
/* ============================================================
 * bootstrap-dropdown.js v2.0.4
 * http://twitter.github.com/bootstrap/javascript.html#dropdowns
 * ============================================================
 * Copyright 2012 Twitter, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ============================================================ */


!function ($) {

	"use strict"; // jshint ;_;


	/* DROPDOWN CLASS DEFINITION
	 * ========================= */

	var toggle = '[data-toggle="dropdown"]'
		, Dropdown = function (element) {
			var $el = $(element).on('click.dropdown.data-api', this.toggle)
			$('html').on('click.dropdown.data-api', function () {
				$el.parent().removeClass('open')
			})
		}

	Dropdown.prototype = {

		constructor: Dropdown

		, toggle: function (e) {
			var $this = $(this)
				, $parent
				, selector
				, isActive

			if ($this.is('.disabled, :disabled')) return

			selector = $this.attr('data-target')

			if (!selector) {
				selector = $this.attr('href')
				selector = selector && selector.replace(/.*(?=#[^\s]*$)/, '') //strip for ie7
			}

			$parent = $(selector)
			$parent.length || ($parent = $this.parent())

			isActive = $parent.hasClass('open')

			clearMenus()

			if (!isActive) $parent.toggleClass('open')

			return false
		}

	}

	function clearMenus() {
		$(toggle).parent().removeClass('open')
	}


	/* DROPDOWN PLUGIN DEFINITION
	 * ========================== */

	$.fn.dropdown = function (option) {
		return this.each(function () {
			var $this = $(this)
				, data = $this.data('dropdown')
			if (!data) $this.data('dropdown', (data = new Dropdown(this)))
			if (typeof option == 'string') data[option].call($this)
		})
	}

	$.fn.dropdown.Constructor = Dropdown


	/* APPLY TO STANDARD DROPDOWN ELEMENTS
	 * =================================== */

	$(function () {
		$('html').on('click.dropdown.data-api', clearMenus)
		$('body')
			.on('click.dropdown', '.dropdown form', function (e) { e.stopPropagation() })
			.on('click.dropdown.data-api', toggle, Dropdown.prototype.toggle)
	})

}(window.jQuery);
/* ===========================================================
 * bootstrap-tooltip.js v2.0.4
 * http://twitter.github.com/bootstrap/javascript.html#tooltips
 * Inspired by the original jQuery.tipsy by Jason Frame
 * ===========================================================
 * Copyright 2012 Twitter, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ========================================================== */


!function ($) {

	"use strict"; // jshint ;_;


	/* TOOLTIP PUBLIC CLASS DEFINITION
	 * =============================== */

	var Tooltip = function (element, options) {
		this.init('tooltip', element, options)
	}

	Tooltip.prototype = {

		constructor: Tooltip

		, init: function (type, element, options) {
			var eventIn
				, eventOut

			this.type = type
			this.$element = $(element)
			this.options = this.getOptions(options)
			this.enabled = true

			if (this.options.trigger != 'manual') {
				eventIn  = this.options.trigger == 'hover' ? 'mouseenter' : 'focus'
				eventOut = this.options.trigger == 'hover' ? 'mouseleave' : 'blur'
				this.$element.on(eventIn, this.options.selector, $.proxy(this.enter, this))
				this.$element.on(eventOut, this.options.selector, $.proxy(this.leave, this))
			}

			this.options.selector ?
				(this._options = $.extend({}, this.options, { trigger: 'manual', selector: '' })) :
				this.fixTitle()
		}

		, getOptions: function (options) {
			options = $.extend({}, $.fn[this.type].defaults, options, this.$element.data())

			if (options.delay && typeof options.delay == 'number') {
				options.delay = {
					show: options.delay
					, hide: options.delay
				}
			}

			return options
		}

		, enter: function (e) {
			var self = $(e.currentTarget)[this.type](this._options).data(this.type)

			if (!self.options.delay || !self.options.delay.show) return self.show()

			clearTimeout(this.timeout)
			self.hoverState = 'in'
			this.timeout = setTimeout(function() {
				if (self.hoverState == 'in') self.show()
			}, self.options.delay.show)
		}

		, leave: function (e) {
			var self = $(e.currentTarget)[this.type](this._options).data(this.type)

			if (this.timeout) clearTimeout(this.timeout)
			if (!self.options.delay || !self.options.delay.hide) return self.hide()

			self.hoverState = 'out'
			this.timeout = setTimeout(function() {
				if (self.hoverState == 'out') self.hide()
			}, self.options.delay.hide)
		}

		, show: function () {
			var $tip
				, inside
				, pos
				, actualWidth
				, actualHeight
				, placement
				, tp

			if (this.hasContent() && this.enabled) {
				$tip = this.tip()
				this.setContent()

				if (this.options.animation) {
					$tip.addClass('fade')
				}

				placement = typeof this.options.placement == 'function' ?
					this.options.placement.call(this, $tip[0], this.$element[0]) :
					this.options.placement

				inside = /in/.test(placement)

				$tip
					.remove()
					.css({ top: 0, left: 0, display: 'block' })
					.appendTo(inside ? this.$element : document.body)

				pos = this.getPosition(inside)

				actualWidth = $tip[0].offsetWidth
				actualHeight = $tip[0].offsetHeight

				switch (inside ? placement.split(' ')[1] : placement) {
					case 'bottom':
						tp = {top: pos.top + pos.height, left: pos.left + pos.width / 2 - actualWidth / 2}
						break
					case 'top':
						tp = {top: pos.top - actualHeight, left: pos.left + pos.width / 2 - actualWidth / 2}
						break
					case 'left':
						tp = {top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left - actualWidth}
						break
					case 'right':
						tp = {top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left + pos.width}
						break
				}

				$tip
					.css(tp)
					.addClass(placement)
					.addClass('in')
			}
		}

		, isHTML: function(text) {
			// html string detection logic adapted from jQuery
			return typeof text != 'string'
				|| ( text.charAt(0) === "<"
				&& text.charAt( text.length - 1 ) === ">"
				&& text.length >= 3
				) || /^(?:[^<]*<[\w\W]+>[^>]*$)/.exec(text)
		}

		, setContent: function () {
			var $tip = this.tip()
				, title = this.getTitle()

			$tip.find('.tooltip-inner')[this.isHTML(title) ? 'html' : 'text'](title)
			$tip.removeClass('fade in top bottom left right')
		}

		, hide: function () {
			var that = this
				, $tip = this.tip()

			$tip.removeClass('in')

			function removeWithAnimation() {
				var timeout = setTimeout(function () {
					$tip.off($.support.transition.end).remove()
				}, 500)

				$tip.one($.support.transition.end, function () {
					clearTimeout(timeout)
					$tip.remove()
				})
			}

			$.support.transition && this.$tip.hasClass('fade') ?
				removeWithAnimation() :
				$tip.remove()
		}

		, fixTitle: function () {
			var $e = this.$element
			if ($e.attr('title') || typeof($e.attr('data-original-title')) != 'string') {
				$e.attr('data-original-title', $e.attr('title') || '').removeAttr('title')
			}
		}

		, hasContent: function () {
			return this.getTitle()
		}

		, getPosition: function (inside) {
			return $.extend({}, (inside ? {top: 0, left: 0} : this.$element.offset()), {
				width: this.$element[0].offsetWidth
				, height: this.$element[0].offsetHeight
			})
		}

		, getTitle: function () {
			var title
				, $e = this.$element
				, o = this.options

			title = $e.attr('data-original-title')
				|| (typeof o.title == 'function' ? o.title.call($e[0]) :  o.title)

			return title
		}

		, tip: function () {
			return this.$tip = this.$tip || $(this.options.template)
		}

		, validate: function () {
			if (!this.$element[0].parentNode) {
				this.hide()
				this.$element = null
				this.options = null
			}
		}

		, enable: function () {
			this.enabled = true
		}

		, disable: function () {
			this.enabled = false
		}

		, toggleEnabled: function () {
			this.enabled = !this.enabled
		}

		, toggle: function () {
			this[this.tip().hasClass('in') ? 'hide' : 'show']()
		}

	}


	/* TOOLTIP PLUGIN DEFINITION
	 * ========================= */

	$.fn.tooltip = $.fn.bstooltip = function ( option ) {
		return this.each(function () {
			var $this = $(this)
				, data = $this.data('tooltip')
				, options = typeof option == 'object' && option;
			if (!data) $this.data('tooltip', (data = new Tooltip(this, options)));
			if (typeof option == 'string') data[option]();
		})
	}

	$.fn.tooltip.Constructor = $.fn.bstooltip.Constructor = Tooltip;

	$.fn.tooltip.defaults = $.fn.bstooltip.defaults = {
		animation: true
		, placement: 'top'
		, selector: false
		, template: '<div class="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>'
		, trigger: 'hover'
		, title: ''
		, delay: 0
	}

}(window.jQuery);
/* =========================================================
 * bootstrap-modal.js v2.0.1
 * http://twitter.github.com/bootstrap/javascript.html#modals
 * =========================================================
 * Copyright 2012 Twitter, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ========================================================= */


!function( $ ){

 	"use strict"

 /* MODAL CLASS DEFINITION
  * ====================== */

	var Modal = function ( content, options ) {
		this.options = options
		this.$element = $(content).delegate('[data-dismiss="modal"]', 'click.dismiss.modal', $.proxy(this.hide, this))
	}

	Modal.prototype = {

		constructor: Modal,

		toggle: function () {
			return this[!this.isShown ? 'show' : 'hide']()
		},

		show: function () {
			var that = this

			if (this.isShown) return

			$('body').addClass('modal-open')

			setTitle.call(this)
			bindAjax.call(this)

			this.isShown = true
			this.$element.trigger('show')

			escape.call(this)
			backdrop.call(this, function () {
				var transition = $.support.transition && that.$element.hasClass('fade')

				!that.$element.parent().length && that.$element.appendTo(document.body) //don't move modals dom position

				that.$element.show()

				if (transition) {
					that.$element[0].offsetWidth // force reflow
				}

				that.$element.addClass('in')

				transition ?
				that.$element.one($.support.transition.end, function () { that.$element.trigger('shown') }) :
				that.$element.trigger('shown')
			})
		},

		hide: function ( e ) {
			e && e.preventDefault()

			if (!this.isShown) return

			var that = this
			this.isShown = false

			$('body').removeClass('modal-open')

			escape.call(this)

			this.$element.trigger('hide').removeClass('in')

			$.support.transition && this.$element.hasClass('fade') ?
			hideWithTransition.call(this) :
			hideModal.call(this)
		}
	}


 /* MODAL PRIVATE METHODS
  * ===================== */

	// Method to bind ajax events
	function bindAjax() {
		if (!this.options.url) return

		var that = this;

		this.$element.one('show', function(event){
			that.$element.find('.modal-body, .modal-footer').remove()
			$('<div class="modal-body"><div class="modal-loader"></div></div>').appendTo(that.$element)
		});

		this.$element.one('shown', function(event){
			$.get(that.options.url, function(data){
				that.$element.find('.modal-body').remove()
				that.$element.append(data)
			});
		});
	}

	// Method to set title
	function setTitle() {
		if(!this.options.title) return
		var header = this.$element.find('.modal-header');
		if(header.find('h3').length == 0) {
			$('<h3>'+this.options.title+'</h3>').appendTo(header)
		} else {
			header.find('h3').text(this.options.title)
		}
	}


  function hideWithTransition() {
    var that = this
      , timeout = setTimeout(function () {
          that.$element.off($.support.transition.end)
          hideModal.call(that)
        }, 500)

    this.$element.one($.support.transition.end, function () {
      clearTimeout(timeout)
      hideModal.call(that)
    })
  }

  function hideModal( that ) {
    this.$element
      .hide()
      .trigger('hidden')

    backdrop.call(this)
  }

  function backdrop( callback ) {
    var that = this
      , animate = this.$element.hasClass('fade') ? 'fade' : ''

    if (this.isShown && this.options.backdrop) {
      var doAnimate = $.support.transition && animate

      this.$backdrop = $('<div class="modal-backdrop ' + animate + '" />')
        .appendTo(document.body)

      if (this.options.backdrop != 'static') {
        this.$backdrop.click($.proxy(this.hide, this))
      }

      if (doAnimate) this.$backdrop[0].offsetWidth // force reflow

      this.$backdrop.addClass('in')

      doAnimate ?
        this.$backdrop.one($.support.transition.end, callback) :
        callback()

    } else if (!this.isShown && this.$backdrop) {
      this.$backdrop.removeClass('in')

      $.support.transition && this.$element.hasClass('fade')?
        this.$backdrop.one($.support.transition.end, $.proxy(removeBackdrop, this)) :
        removeBackdrop.call(this)

    } else if (callback) {
      callback()
    }
  }

  function removeBackdrop() {
    this.$backdrop.remove();
    this.$backdrop = null
  }

  function escape() {
    var that = this
    if (this.isShown && this.options.keyboard) {
      $(document).on('keyup.dismiss.modal', function ( e ) {
        e.which == 27 && that.hide()
      })
    } else if (!this.isShown) {
      $(document).off('keyup.dismiss.modal')
    }
  }


 /* MODAL PLUGIN DEFINITION
  * ======================= */

	$.fn.modal = function (option, apiData) {
		return this.each(function () {
			var $this = $(this),
				data = $this.data('modal'),
				options = $.extend({}, $.fn.modal.defaults, $this.data(), typeof option == 'object' && option)
			if (!data) $this.data('modal', (data = new Modal(this, options)))
			else if(apiData) data.options = $.extend(data.options, apiData)
			if (typeof option == 'string') data[option]()
			else if (options.show) data.show()
		})
	}

	$.fn.modal.defaults = {
		backdrop: true,
		keyboard: true,
		show: true
	}

	$.fn.modal.Constructor = Modal


 /* MODAL DATA-API
  * ============== */

	$(function () {
		$('body').on('click.modal.data-api', '[data-toggle="modal"]', function ( e ) {
			var $this = $(this),
				href,
				$target = $($this.attr('data-target') || (href = $this.attr('href')) && href.replace(/.*(?=#[^\s]+$)/, '')), //strip for ie7
				option = $target.data('modal') ? 'toggle' : $.extend({}, $target.data(), $this.data())
			e.preventDefault()
			$target.modal(option, $this.data())
		})
	})

}( window.jQuery );

/**
 * jQuery.ScrollTo - Easy element scrolling using jQuery.
 * @version 1.4.2
 * http://flesler.blogspot.com/2007/10/jqueryscrollto.html
 */
;(function(d){var k=d.scrollTo=function(a,i,e){d(window).scrollTo(a,i,e)};k.defaults={axis:'xy',duration:parseFloat(d.fn.jquery)>=1.3?0:1};k.window=function(a){return d(window)._scrollable()};d.fn._scrollable=function(){return this.map(function(){var a=this,i=!a.nodeName||d.inArray(a.nodeName.toLowerCase(),['iframe','#document','html','body'])!=-1;if(!i)return a;var e=(a.contentWindow||a).document||a.ownerDocument||a;return d.browser.safari||e.compatMode=='BackCompat'?e.body:e.documentElement})};d.fn.scrollTo=function(n,j,b){if(typeof j=='object'){b=j;j=0}if(typeof b=='function')b={onAfter:b};if(n=='max')n=9e9;b=d.extend({},k.defaults,b);j=j||b.speed||b.duration;b.queue=b.queue&&b.axis.length>1;if(b.queue)j/=2;b.offset=p(b.offset);b.over=p(b.over);return this._scrollable().each(function(){var q=this,r=d(q),f=n,s,g={},u=r.is('html,body');switch(typeof f){case'number':case'string':if(/^([+-]=)?\d+(\.\d+)?(px|%)?$/.test(f)){f=p(f);break}f=d(f,this);case'object':if(f.is||f.style)s=(f=d(f)).offset()}d.each(b.axis.split(''),function(a,i){var e=i=='x'?'Left':'Top',h=e.toLowerCase(),c='scroll'+e,l=q[c],m=k.max(q,i);if(s){g[c]=s[h]+(u?0:l-r.offset()[h]);if(b.margin){g[c]-=parseInt(f.css('margin'+e))||0;g[c]-=parseInt(f.css('border'+e+'Width'))||0}g[c]+=b.offset[h]||0;if(b.over[h])g[c]+=f[i=='x'?'width':'height']()*b.over[h]}else{var o=f[h];g[c]=o.slice&&o.slice(-1)=='%'?parseFloat(o)/100*m:o}if(/^\d+$/.test(g[c]))g[c]=g[c]<=0?0:Math.min(g[c],m);if(!a&&b.queue){if(l!=g[c])t(b.onAfterFirst);delete g[c]}});t(b.onAfter);function t(a){r.animate(g,j,b.easing,a&&function(){a.call(this,n,b)})}}).end()};k.max=function(a,i){var e=i=='x'?'Width':'Height',h='scroll'+e;if(!d(a).is('html,body'))return a[h]-d(a)[e.toLowerCase()]();var c='client'+e,l=a.ownerDocument.documentElement,m=a.ownerDocument.body;return Math.max(l[h],m[h])-Math.min(l[c],m[c])};function p(a){return typeof a=='object'?a:{top:a,left:a}}})(jQuery);

/**
 * jQuery.ScrollSpy adaptation of David Walsh MooTools ScrollSpy
 * https://github.com/sxalexander/jquery-scrollspy
 */
;(function($,window,document,undefined){$.fn.extend({scrollspy:function(options){var defaults={min:0,max:0,mode:'vertical',buffer:0,container:window,onEnter:options.onEnter?options.onEnter:[],onLeave:options.onLeave?options.onLeave:[],onTick:options.onTick?options.onTick:[]}; var options=$.extend({},defaults,options);return this.each(function(i){var element=this;var o=options;var $container=$(o.container);var mode=o.mode;var buffer=o.buffer;var enters=leaves=0;var inside=false;$container.bind('scroll',function(e){var position={top:$(this).scrollTop(),left:$(this).scrollLeft()};var xy=(mode=='vertical')?position.top+buffer:position.left+buffer;var max=o.max;var min=o.min;if($.isFunction(o.max)){max=o.max()}if($.isFunction(o.min)){min=o.min()}if(max==0){max=(mode=='vertical')?$container.height():$container.outerWidth()+$(element).outerWidth()}if(xy>=o.min&&xy<=max){if(!inside){inside=true;enters++;$(element).trigger('scrollEnter',{position:position});if($.isFunction(o.onEnter)){o.onEnter(element,position)}}$(element).trigger('scrollTick',{position:position,inside:inside,enters:enters,leaves:leaves});if($.isFunction(o.onTick)){o.onTick(element,position,inside,enters,leaves)}}else{if(inside){inside=false;leaves++;$(element).trigger('scrollLeave',{position:position,leaves:leaves});if($.isFunction(o.onLeave)){o.onLeave(element,position)}}}})})}})})(jQuery,window);

/**
 * jQuery.Cookie Plugin
 * @version v1.3
 * https://github.com/carhartl/jquery-cookie
 */
;(function($,document,undefined){var pluses=/\+/g;function raw(s){return s}function decoded(s){return decodeURIComponent(s.replace(pluses,' '))}var config=$.cookie=function(key,value,options){if(value!==undefined){options=$.extend({},config.defaults,options);if(value===null){options.expires=-1}if(typeof options.expires==='number'){var days=options.expires,t=options.expires=new Date();t.setDate(t.getDate()+days)}value=config.json?JSON.stringify(value):String(value);return(document.cookie=[encodeURIComponent(key),'=',config.raw?value:encodeURIComponent(value),options.expires?'; expires='+options.expires.toUTCString():'',options.path?'; path='+options.path:'',options.domain?'; domain='+options.domain:'',options.secure?'; secure':''].join(''))}var decode=config.raw?raw:decoded;var cookies=document.cookie.split('; ');for(var i=0,parts;(parts=cookies[i]&&cookies[i].split('='));i++){if(decode(parts.shift())===key){var cookie=decode(parts.join('='));return config.json?JSON.parse(cookie):cookie}}return null};config.defaults={};$.removeCookie=function(key,options){if($.cookie(key)!==null){$.cookie(key,null,options);return true}return false}})(jQuery,document);

/*******************************************************************************
 * Main Website Javascript object
 *******************************************************************************/
var Mem;

Mem = {



};








/*******************************************************************************
* Init vars
*******************************************************************************/

var FunPosts, Posts, Share;

$.browser.chrome = $.browser.webkit && !!window.chrome;
$.browser.safari = $.browser.webkit && !window.chrome;

/*******************************************************************************
* Main Engine Init
*******************************************************************************/
FunPosts = {
	is_logged: false,

	init: function() {
		// Likes
		/*if(FunPosts.is_logged) {
			$('a.like, a.dislike').click(Posts.like);
		}*/
		// Config ajax
		$.ajaxSetup({
			cache: false
		});
		// Home button
		Stl.init();
	},

	addCommas: function(nStr)
	{
		nStr += '';
		x = nStr.split('.');
		x1 = x[0];
		x2 = x.length > 1 ? '.' + x[1] : '';
		var rgx = /(\d+)(\d{3})/;
		while (rgx.test(x1)) {
			x1 = x1.replace(rgx, '$1' + ',' + '$2');
		}
		return x1 + x2;
	}
}

$(document).ready(FunPosts.init);

/*******************************************************************************
 * Posts
 *******************************************************************************/
var Stl = {

	init: function() {
		// Check for container
		var el = $('#stl_left');

		// Stop if we have no container
		if( ! Stl.offset()) return;

		// Element Events
		el.click    (function(){ $(window).scrollTop(0); });
		el.mouseover(function(){ el.addClass('over');    });
		el.mouseout (function(){ el.removeClass('over'); });

		// Fire events to init
		Stl.resize();

		// Window events
		$(window).resize(Stl.resize);
		$(window).scroll(Stl.resize);
	},

	resize: function() {
		// Init vars
		var el     = $('#stl_left'),
			wWidth = $(window).width(),
			pos    = Stl.offset(),
			scroll = $(window).scrollTop();

		// Stop if we have no container
		if( ! pos) return;

		// Set maximum height
		el.css('height', $(window).height());

		// Set text alignment
		/*if(pos < 100) {
			$('#stl_text_horiz').css('display', 'none');
			$('#stl_text_verti').css('display', 'block');
		} else {
			$('#stl_text_horiz').css('display', 'block');
			$('#stl_text_verti').css('display', 'none');
		}*/

		// Hide for too small
		if(pos < 39) {
			el.css('display', 'none');
		}

		// Set the correct size
		else {
			el.css('display', 'block');
			el.css('width', pos);
		}

		// Check scroll position
		if(scroll < 300) {
			el.css('display', 'none');
		}
	},

	offset: function() {
		// Init vars
		var cont = $('#body_container');

		// Check if container exists
		if( ! cont.length) return;

		// Get position
		if($.browser.chrome) {
			return parseInt(cont.css('margin-left'));
		} else {
			return parseInt(cont.position().left);
		}
	}

};

/*******************************************************************************
* Posts
*******************************************************************************/

Posts = {
	like: function(el) {
		// Prevent default event
		//event.preventDefault();
		// Wrap Jquery
		var $this = $(el);
		var pid = $this.attr('data-pid');
		// Unlike / Undislike
		if($this.hasClass('liked') || $this.hasClass('disliked')) {
			$this.removeClass('liked').removeClass('disliked');
			// Get votes
			$.getJSON('/vote/unlike/'+pid+'?ajax=1', function(data){
				if(data.flood) {
					alert('Превышен лимит текущего действия. Переждите минутку.'); return;
				}
				$('#score'+data.pid).text(data.score);
			});
			return
		}
		// Remove like/dislike if already is on this post
		if($this.hasClass('like')) {
			$('#controls'+pid).find('a.dislike').removeClass('disliked')
		} else {
			$('#controls'+pid).find('a.like').removeClass('liked')
		}
		// Set like or dislike
		if($this.hasClass('like')) {
			$this.addClass('liked');
		} else {
			$this.addClass('disliked');
		}
		// Set votes
		$.getJSON('/vote/'+($this.hasClass('like')?'like':'dislike')+'/'+pid+'?ajax=1', function(data){
			if(data.flood) {
				alert('Превышен лимит текущего действия. Переждите минутку.'); return;
			}
			$('#score'+data.pid).text(data.score);
		});
	},
	report: function(pid) {
		$.post('/report/post/'+pid, $('#report_form').serialize())
		$('#ajaxModal').modal('hide')
	},
	likeRedirect: function(like, pid) {
		var action = like ? 'like%2F'+pid : 'dislike%2F'+pid,
			el1 = $('#modalLikeSignup'),
			el2 = $('#modalLikeSignin'),
			expr = /((dis)?like\%2F[0-9]+)/i;

		el1.attr('href', el1.attr('href').replace(expr, action));
		el2.attr('href', el2.attr('href').replace(expr, action));
	}
}

var PostShare = {

	openPopup: function(el, pid, txt, url) {

		// Check locking
		if(this.locked == true) return;

		// Init elements
		this.btn       = $(el);
		this.post_id   = pid;
		this.post_text = txt;
		this.post_url  = url;

		this.vkSession = VK.Auth.getSession();
		this.vkModal   = $('#vkSharePost');
		this.vkButton  = this.vkModal.find('.share-vk');

		// Lock
		this.lock();

		// Bind OK
		this.vkModal.find('.share-ok').unbind('click').bind('click', function(e){
			e.preventDefault();
			Share.odnoklassniki(url, txt);
		});

		// Check vk
		if(this.vkSession == null) {
			this.unlock();
			this.vkModal.modal('show');
			this.bindVkLogin();
		}

		// Upload pic before showing popup
		else {

			var that = this;

			// Request permissions
			VK.Api.call('account.getAppPermissions', {uid: this.vkSession.mid}, function(data) {
				if( ! data.error) {
					// Check permissions
					that.vkSession.perms = data.response;
					if((that.vkSession.perms & 4) == 0 || (that.vkSession.perms & 8192) == 0) {
						that.unlock();
						that.vkModal.modal('show');
						that.bindVkLogin();
						return;
					}

					// Upload the stuff
					that.doVkUpload(function(){
						that.unlock();
						that.vkModal.modal('show');
					})
				}
			});

		}

	},

	lock: function() {
		this.locked = true;
		if(this.btn) this.btn.parent().find('.loader').show(); else this.vkButton.parent().find('.loader').show();
	},
	unlock: function() {
		this.locked = false;
		if(this.btn) this.btn.parent().find('.loader').hide(); else this.vkButton.parent().find('.loader').hide();
	},

	bindVkLogin: function() {
		var that = this;
		that.vkButton.unbind('click').bind('click', function(e){
			e.preventDefault();

			that.vkModal.find('.alert').hide();

			VK.Auth.login(function(e){

				if(e.status != 'connected' || e.session == null) {

					that.vkButton.parent().find('.loader').hide();
					that.vkModal.find('.alert.app-not-installed').show();

				} else {

					// Lock the popup button and show the loader in it @todo
					that.vkButton.parent().find('.loader').show();

					// Set sessions data
					that.vkSession = e.session;

					// Upload the stuff
					that.doVkUpload(function(){
						that.vkButton.parent().find('.loader').hide();
						that.vkModal.find('.alert.image-uploaded').show();
					})
				}
			}, 73734);
		});
	},

	doVkUpload: function(callAfterUpload) {
		// Load the stuff
		var that = this;
		VK.Api.call('photos.getWallUploadServer', {}, function(data) {

			if(data.error && data.error.error_code == 7) {
				that.vkButton.parent().find('.loader').hide();
				that.vkModal.find('.alert.app-not-installed').show();
				return;
			}

			$.getJSON('/posts/vk_upload', {pid: that.post_id, serv: data.response.upload_url}, function(data){
				if(data.error) {alert('error');}
				// Save photo to wall
				VK.Api.call("photos.saveWallPhoto", {server: data.server, photo: data.photo, hash: data.hash}, function(data) {
					// Do stuff
					callAfterUpload();
					// Post popup
					that.vkButton.unbind('click').bind('click', function(e){
						e.preventDefault();
						VK.Api.call('wall.post', {
							owner_id: that.vkSession.mid,
							message:  that.post_text,
							attachments: data.response[0].id+','+that.post_url
						}, function() {
							that.vkModal.modal('hide');
							that.unlock();
						});
					});
				});
			});
		});
	}

};

/*******************************************************************************
* Post Share
*******************************************************************************/
Share = {
	vkontakte: function(purl, ptitle, pimg, text) {
		url  = 'http://vkontakte.ru/share.php?';
		url += 'url='          + encodeURIComponent(purl);
		url += '&title='       + encodeURIComponent(ptitle);
		url += '&description=' + encodeURIComponent(text);
		url += '&image='       + encodeURIComponent(pimg);
		url += '&noparse=true';
		Share.popup(url)
	},
	odnoklassniki: function(purl, text) {
		url  = 'http://www.odnoklassniki.ru/dk?st.cmd=addShare&st.s=1';
		url += '&st.comments=' + encodeURIComponent(text);
		url += '&st._surl='    + encodeURIComponent(purl);
		Share.popup(url)
	},
	facebook: function(purl, ptitle, pimg, text) {
		url  = 'http://www.facebook.com/sharer.php?s=100';
		url += '&p[title]='     + encodeURIComponent(ptitle);
		url += '&p[summary]='   + encodeURIComponent(text);
		url += '&p[url]='       + encodeURIComponent(purl);
		url += '&p[images][0]=' + encodeURIComponent(pimg);
		Share.popup(url)
	},
	twitter: function(purl, ptitle) {
		url  = 'http://twitter.com/share?';
		url += 'text='      + encodeURIComponent(ptitle);
		url += '&url='      + encodeURIComponent(purl);
		url += '&counturl=' + encodeURIComponent(purl);
		Share.popup(url)
	},
	mailru: function(purl, ptitle, pimg, text) {
		url  = 'http://connect.mail.ru/share?';
		url += 'url='          + encodeURIComponent(purl);
		url += '&title='       + encodeURIComponent(ptitle);
		url += '&description=' + encodeURIComponent(text);
		url += '&imageurl='    + encodeURIComponent(pimg);
		Share.popup(url)
	},

	popup: function(url) {
		window.open(url,'','toolbar=0,status=0,width=626,height=436');
	}
};

/**
 * Jquery image file upload plugin based on blueimp/jqueryfileupload
 */
!function( $ ){

	"use strict"

	/* CLASS DEFINITION
	 * ===================== */

	var ImageUploader = function(content, options) {

		this.options   = options;
		this.container = $(content);
		this.input     = $('.imageUploadFileInput', this.container);

	};

	ImageUploader.prototype = {

		constructor: ImageUploader,

		initialize: function() {
			var fileData,
				container = this.container,
				options   = this.options;

			// Init file upload
			this.input.fileupload({
				dataType: 'json',
				add: function(e, data) {
					// Hide all errors
					$('p[class^="imageUploadError"]', container).hide();

					// Check file size for 5mb
					if(data.files[0].size > options.maxFileSize) {
						$('p.imageUploadErrorSize', container).show();
						return false;
					}

					// Check file type
					if( ! data.files[0].type.match(/image.*/)) {
						$('p.imageUploadErrorType', container).show();
						return false;
					}

					// Set file name
					$('.imageUploadFileName', container).text(data.files[0].name);

					// Set file data
					container.data('uploadfiledata', data);

					// Everything OK
					$('.imageUploadContainerForm', container).hide();
					$('.imageUploadContainerUploading', container).show();
					$(options.submitTrigger).removeAttr('disabled');
				},
				progress: function(e, data) {
					var progress = parseInt(data.loaded / data.total * 100, 10);
					$('.imageUploadProgressBar .bar', container).css('width', progress+'%');
				},
				done: function(result, textStatus) {
					// Get response
					var response = textStatus.result;
					// Check for success
					if(response.success == 1) {
						options.success(response, container, options);
					}
					// Check for error
					if(response.error) {
						// Hide uploading form
						formReset(container, options);

						if(response.error == 1) {
							$('p.imageUploadErrorSize').show();
						}
						if(response.error == 2) {
							$('p.imageUploadErrorType').show();
						}
						if(response.error == 3) {
							$('p.imageUploadErrorUpload').show();
						}
						if(response.error == 4) {
							$('p.imageUploadErrorTooBig').show();
						}
						if(response.error == 5) {
							$('p.imageUploadErrorTooSmall').show();
						}
					}
				}
			});

			// Bind destroy event
			bindDestroyEvent(this.options.destroyTrigger);

			// Bind submit event
			bindSubmitEvent(this.options.submitTrigger, this.container);

			// Bind form resetters
			bindFormReset(this.container, this.options);
		}
	}


	/* PRIVATE METHODS
	 * ===================== */

	var bindDestroyEvent = function(input) {
		$(input).click(function(){
			$(this).fileupload('destroy')
		});
	};

	var bindSubmitEvent = function(input, container, fileData) {
		$(input).click(function(){
			if( ! $(this).attr('disabled')) {
				$(this).attr('disabled', 'disabled');
				$('p.imageUploadSelectedLabel', container).hide();
				$('p.imageUploadUploadingLabel', container).show();
				container.data('uploadfiledata').submit();
			}
		});
	};

	var bindFormReset = function(container, options) {
		$(options.resetTriggers, container).click(function(){
			formReset(container, options);
		});
	};

	var formReset = function(container, options) {
		$('.imageUploadSelectedLabel', container).show();
		$('.imageUploadUploadingLabel', container).hide();
		$('.imageUploadContainerUploading', container).hide();
		$('.imageUploadContainerForm', container).show();
		$(options.submitTrigger).attr('disabled', 'disabled');
		$('.imageUploadProgressBar .bar', container).css('width', '0%');
	};

	/* PLUGIN DEFINITION
	 * ======================= */

	$.fn.imageuploader = function (option, apiData) {
		return this.each(function () {
			var $this = $(this),
				data = $this.data('imageuploader'),
				options = $.extend({}, $.fn.imageuploader.defaults, $this.data(), typeof option == 'object' && option)

			if (!data) $this.data('imageuploader', (data = new ImageUploader(this, options)));
			else if(apiData) data.options = $.extend(data.options, apiData);

			if (typeof option == 'string') data[option]()
			else data.initialize()
		})
	};

	$.fn.imageuploader.defaults = {
		maxFileSize    : 5120000,
		submitTrigger  : '#imageUploadSubmitTrigger',
		destroyTrigger : '#imageUploadDestroyTrigger',
		resetTriggers  : '.imageUploadFormReset',
		success        : function(){}
	};

	$.fn.imageuploader.Constructor = ImageUploader;

}(window.jQuery);

/*******************************************************************************
 * Profile
 *******************************************************************************/
var Profile = {

	follow: function(username) {
		// Block button
		$('.btn-follow').attr('disabled', true);
		// Send async request
		$.ajax({
			dataType: 'json',
			url: '/'+username+'/follow'
		}).done(function(){
			// Unblock and change buttons
			$('.btn-follow').removeAttr('disabled').hide();
			$('.btn-unfollow').show();
			$('#counter-followers').html(FunPosts.addCommas(parseInt($('#counter-followers').text().split(",").join(""))+1));
		});
	},

	unfollow: function(username) {
		// Block button
		$('.btn-unfollow').attr('disabled', true);
		// Send async request
		$.ajax({
			dataType: 'json',
			url: '/'+username+'/unfollow'
		}).done(function(){
			// Unblock and change buttons
			$('.btn-unfollow').removeAttr('disabled').hide();
			$('.btn-follow').show();
			$('#counter-followers').html(FunPosts.addCommas(parseInt($('#counter-followers').text().split(",").join(""))-1));
		});
	}

};

if(typeof define==='undefined') {var define;}