// copyright Hester Consultants LLC

var hesterConsultants =
{

    // fit the height of the client control panel
    // and width of the main body
    fitClientPanel: function ()
    {
        var MAINBODY_CLIENTS_WIDTH = 230;
        var PADDING = 60;
        var clientControlDiv = $(".clientControlPanel");

        if (!!(clientControlDiv.get(0)))
        {
            var mainbodyClientsDiv = $("#mainbodyClients");
            var hasMainbodyClients = !!(mainbodyClientsDiv.get(0));

            // client panel
            clientControlDiv.hcJq("fillContainerHeight", $("#container"));

            // mainbody
            if (hasMainbodyClients)
                mainbodyClientsDiv.width($(window).width() - (MAINBODY_CLIENTS_WIDTH + PADDING));

            window.onresize = function ()
            {
                clientControlDiv.hcJq("fillContainerHeight", $("#container"));

                if (hasMainbodyClients)
                    mainbodyClientsDiv.width($(window).width() - (MAINBODY_CLIENTS_WIDTH + PADDING));
            }
        }
    },

    setupSearchInputs: function ()
    {
        // make enter key submit search
        if (!!($(".searchText").get(0)) && !!($(".searchButton").get(0)))
        {
            $(".searchText").keypress(function (e)
            {
                if (e.which == 13)
                    e.preventDefault();
            });

            $(".searchText").keyup(function (e)
            {
                if (e.which == 13)
                {
                    e.preventDefault();
                    $(".searchButton").click();
                }
            });
        }
    },

    setupClientPanel: function ()
    {
        this.fitClientPanel();
        this.setupSearchInputs();
    },

    roundToDecimals: function (num, decimalPlaces)
    {
        var result = Math.round(num * Math.pow(10, decimalPlaces)) / Math.pow(10, decimalPlaces);
        return result;
    }
}