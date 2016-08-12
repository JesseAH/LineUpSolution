

app.controller("roundDetailsCtrl", function ($scope, $rootScope, $routeParams, $http, DefaultFactory) {

    //#region Variables

    $scope.detailsScope = {
        detailsObject: null,
        header: "Getting Round Information... ",
        roundID: $routeParams.roundID,
        leagueTeamID: $routeParams.leagueTeamID
    };

    $scope.valid = true;
    $scope.picks = [];

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

    function createPick (matchID, pickedID, points)
    {
        return {
            id: 0,
            match_id: matchID,
            league_team_id: $scope.detailsScope.leagueTeamID,
            picked_team_id: pickedID,
            confidence_value: points

        }
    }

    $scope.pointSelected = function (matchID, pickedID, points)
    {
        var pick = createPick(matchID, pickedID, points);

        //remove picks with same match_id
        $scope.picks = $.grep($scope.picks, function (item) {
            return item.match_id !== matchID;
        });

        //add new pick
        $scope.picks.push(pick);
        $scope.valid = validPickCount()
    }

    function validPickCount()
    {
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
                    if (hist[a.confidence_value] > $scope.detailsScope.detailsObject.max_pick_count) {
                        returnVal = false;
                    }

                } else hist[a.confidence_value] = 1;

       
            });
        });

        return returnVal;
    }

    $scope.savePicks = function()
    {

        DefaultFactory.Save($scope.picks, "Pick")
        .then(function (result) {

            if(result.data == true)
                window.location.href = '../user/team/details/' + $scope.detailsScope.leagueTeamID;
        });

    }

});

