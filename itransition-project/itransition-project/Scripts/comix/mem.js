/*******************************************************************************
 * New scripts
 *******************************************************************************/
var Mem = {

	Auth: {
		popup: function(service, query_string) {
			if(query_string) query_string = '&'+query_string; else query_string = '';
			window.open('/signin/service?api='+service + query_string, 'oauth_popup', "width=620,height=350,resizable=yes,location=yes,left=100,top=100").focus();
		}
	},

    /**
     * Notifications
     */
    Notifications : {

        _inited:   null,
        _trigger:  null,
        _loader:   null,
        _content:  null,
        _dropdown: null,
        _empty:    null,

        bindCloseEvent: function() {
            $(document).click(function(e){
                var id  = 'notification-dropdown',
                    tr  = 'notification-dropdown-trigger',
                    tar = $(e.target),
                    eid = tar.attr('id');

                if(eid !== tr && eid !== id && tar.parents('#'+tr).length == 0 && tar.parents('#'+id).length == 0) {
                    Mem.Notifications.close()
                }
            });
        },

        load: function() {
            var self = this;
            if( ! this._inited) {
                this._trigger  = $('#notification-dropdown-trigger');
                this._loader   = $('#notification-loader');
                this._content  = $('#notification-content');
                this._dropdown = $('#notification-dropdown');
                this._empty    = $('#notification-empty');
                this._inited   = true;

                $(document).on('show.bs.dropdown', function() {
                    self.close()
                });
            }
        },

        toggle: function() {
            this.load();
            if(this._trigger.hasClass('open')) {
                this.close();
            } else {
                this._trigger.addClass('open');
                $('.label', this._trigger).hide();
                this._dropdown.show();
                $.ajax({
                    dataType: 'json',
                    url: '/stream/notifications'
                }).done(function(data){
                    var that = Mem.Notifications;
                    that._loader.hide();
                    if(data.results == 0) {
                        that._empty.show();
                    } else {
                        that._content.html(data.html).show();
                    }
                });
            }
        },

        close: function() {
            this.load();
            if(this._trigger.hasClass('open')) {
                this._trigger.removeClass('open');
                this._dropdown.hide();
                this._empty.hide();
                this._content.empty().hide();
                this._loader.show();
            }
        },

        delete: function(id) {
            $('#notification'+id).remove();
            if($('#notification-dropdown ul > li').length == 0) {
                this.close();
                this.toggle();
            }
            $.ajax({
                dataType: 'json',
                url: '/stream/notifications_delete/'+id
            });
        }

    },

};