// jquery plugin

(function ($)
{
    var HEADER_HEIGHT = 85;
    var FOOTER_HEIGHT = 24;
    var MAINBODY_PADDING = 30;
    var HEADER_BORDER = 1;
    var FOOTER_BORDER = 1;

    var methods =
    {
        init: function ()
        {
        },

        fillContainerHeight: function (container)
        {
            return this.each(function ()
            {
                $(this).height(container.height() - (HEADER_HEIGHT + FOOTER_HEIGHT + HEADER_BORDER + FOOTER_BORDER));
            });
        }
    };

    $.fn.hcJq = function (method)
    {
        if (!!methods[method])
        {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        }
        else if (typeof method === 'object' || !method)
            return methods.init.apply(this, arguments);
        else
            $.error('Method ' + method + ' does not exist on jQuery.hcJq');
    }
})(jQuery);