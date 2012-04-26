        $(document).ready(function () {
            $("#AdvancedSearch").dialog({
                autoOpen: false,
                width: 900,
                height: 450,
                show: "Fade",
                hide: "explode"
            });
            $("#AdvSearch").click(function () {
                document.getElementById("AdvancedSearch").style.display = "inline";
                $("#AdvancedSearch").dialog("open");
                return false;
            });

            $('#id_search').quicksearch('table tbody tr',
            {
                noResults: '#noresults',
                stripeRows: ['odd', 'even'],
                loader: 'img.loading'
            });
        });

        function CloseDialog() {
            $("#AdvancedSearch").dialog("close");
        }

        function hideSearch() {
            $("#search").hide();
        }
