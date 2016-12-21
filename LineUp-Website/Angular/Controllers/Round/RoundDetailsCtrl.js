

app.controller("roundDetailsCtrl", function ($scope, $rootScope, $routeParams, $http, DefaultFactory) {

    toastr.options = {
        "closeButton": true,
        "showDuration": "300"
    };

    //#region Variables

    $scope.detailsScope = {
        detailsObject: null,
        header: "Getting Round Information... ",
        roundID: $routeParams.roundID,
        leagueTeamID: $routeParams.leagueTeamID
    };

    $scope.saving = false;
    $scope.loadingCompleted = false;
    $scope.valid = true;
    $scope.picks = [];
    $scope.maxPicksReached1 = false;
    $scope.maxPicksReached2 = false;
    $scope.maxPicksReached3 = false;
    $scope.maxPicksReached4 = false;
    $scope.maxPicksReached5 = false;

    //#endregion

    //#region Get Data

    $http.get('../Round/Details/' + $scope.detailsScope.roundID + '/' + $scope.detailsScope.leagueTeamID)
   .then(function (result) {
       var dto = result.data;

       $scope.detailsScope.detailsObject = dto;
       $scope.detailsScope.header = dto.name;

       dto.matches.forEach(function (match) {
           var new_ui_picks =
               [{ match_id: match.id, team_id: match.team1_id, confidence_value: getPickConfidenceValue(match, match.team1_id) },
               { match_id: match.id, team_id: match.team2_id, confidence_value: getPickConfidenceValue(match, match.team2_id) }];
           match.ui_picks = new_ui_picks;
       });

       $scope.displayedCollection = dto.matches;
       $scope.loadingCompleted = true;
   })
   .catch(function (error) {
   });


    function getPickConfidenceValue(match, selectedTeamID) {

        if (match.picks[0] && match.picks[0].picked_team_id == selectedTeamID) {

            var pick = createPick(match.id, match.picks[0].picked_team_id, match.picks[0].confidence_value);
            $scope.picks.push(pick);
            return match.picks[0].confidence_value;
        }
   
        return null;
    }

    //#endregion



    $scope.pointSelected = function (match, points, choice)
    {
        if (choice === 'Option A')
        {
            match.ui_picks[0].confidence_value = points;
            match.ui_picks[1].confidence_value = null;

            removePicks(match.id);
            addNewPick(match.id, match.team1_id, points);
        }
        else if (choice === 'Option B')
        {
            match.ui_picks[1].confidence_value = points;
            match.ui_picks[0].confidence_value = null;

            removePicks(match.id);
            addNewPick(match.id, match.team2_id, points);
        }
        else {
            match.ui_picks[1].confidence_value = null;
            match.ui_picks[0].confidence_value = null;
            removePicks(match.id);
        }

        $scope.valid = validPickCount();
    }

    //Add pick to match
    function addNewPick(matchID, pickedID, points)
    {
        var pick = createPick(matchID, pickedID, points);

        //add new pick
        $scope.picks.push(pick);
    }

    //Remove picks from match
    function removePicks(matchID)
    {
        //remove picks with same match_id
        $scope.picks = $.grep($scope.picks, function (item) {
            return item.match_id !== matchID;
        });
    }

    //Create new pick object
    function createPick(matchID, pickedID, points) {

        return {
            id: 0,
            match_id: matchID,
            league_team_id: $scope.detailsScope.leagueTeamID,
            picked_team_id: pickedID,
            confidence_value: points

        }
    }
    
    //check validate pick count
    function validPickCount()
    {
        $scope.maxPicksReached1 = false;
        $scope.maxPicksReached2 = false;
        $scope.maxPicksReached3 = false;
        $scope.maxPicksReached4 = false;
        $scope.maxPicksReached5 = false;
        var returnVal = true;
        var valid = true;
        var hist = {};

        //check against max pick count
        $scope.displayedCollection.map(function (m)
        {
            m.ui_picks.map(function (a) {

                if (a.confidence_value == null)
                    return;

                if (a.confidence_value in hist) {
                    hist[a.confidence_value]++;
                    if (hist[a.confidence_value] == $scope.detailsScope.detailsObject.max_pick_count) {

                        switch (a.confidence_value) {
                            case 1:
                                $scope.maxPicksReached1 = true;
                                break;
                            case 2:
                                $scope.maxPicksReached2 = true;
                                break;
                            case 3:
                                $scope.maxPicksReached3 = true;
                                break;
                            case 4:
                                $scope.maxPicksReached4 = true;
                                break;
                            case 5:
                                $scope.maxPicksReached5 = true;
                                break;
                        }
                    }

                    if (hist[a.confidence_value] > $scope.detailsScope.detailsObject.max_pick_count) {
                        returnVal = false;
                    }

                }
                else {
                    hist[a.confidence_value] = 1;
                } 

       
            });
        });

        return returnVal;
    }

    $scope.savePicks = function()
    {
        $scope.saving = true;
        DefaultFactory.Save($scope.picks, "Pick")
        .then(function (result) {

            if (result.data == true)
                window.location.href = '../user/team/details/' + $scope.detailsScope.leagueTeamID;
            else
                toastr.error('You do not have permission to make picks for this team!');

            $scope.saving = false;
        });

    }

});

