
var myApp = angular.module('SaleApp', []);
//Defining a Controller 
myApp.controller('PropertyController', function ($scope, $http, $filter) {

    // Get all Property list for dropdownlist
    $scope.Error = "";
    var orderBy = $filter('orderBy');
    $("#loading").show();

    // CalculatePrice
    $scope.PropertyPricingInit = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/PlanCtrl/GetPlanTypeMasterList',
        }).success(function (data) {
            $scope.PlanTypeMasterList = data;
        })

        var flatid = $("#hidFlatID").val();
        $http({
            method: 'Get',
            url: '/Sale/Property/GetFlatDetails',
            params: { flatid: flatid },
        }).success(function (data) {
            $scope.FlatDetails = data;
            var totalPLC = 0;
            var GtotalPLC = 0;
            var FlatSize = 0;
            var TotalFloorPlc = 0;
            var GTotalFloorPlc = 0;
            var GAdditionalCharge = 0;
            $scope.FlatSize = $scope.FlatDetails.FlatSize;
            for (var i = 0; i < $scope.FlatDetails.FlatPLCList.length; i++) {
                var list = $scope.FlatDetails.FlatPLCList[i];
                totalPLC += list.AmountSqFt;
                GtotalPLC += list.TotalAmount;
            }
            $scope.TotalPLCAmount = totalPLC;
            $scope.GTotalPLCAmount = GtotalPLC;
            var totalCharge = 0;
            var GtotalCharge = 0;
            for (var i = 0; i < $scope.FlatDetails.FlatChargeList.length; i++) {
                var list = $scope.FlatDetails.FlatChargeList[i];
                totalCharge += list.Amount;
                GtotalCharge += list.TotalAmount;
            }
            debugger;
            for (var i = 0; i < $scope.FlatDetails.FloorWisePlc.length; i++) {
                var list = $scope.FlatDetails.FloorWisePlc[i];
                TotalFloorPlc = parseFloat(TotalFloorPlc) + list.AmountSqFt;
                GTotalFloorPlc = parseFloat(GTotalFloorPlc) + parseFloat(list.AmountSqFt) * parseFloat($scope.FlatDetails.FlatSize);
            }
            for (var i = 0; i < $scope.FlatDetails.AdditionalCharge.length; i++) {
                var list = $scope.FlatDetails.AdditionalCharge[i];
                GAdditionalCharge = parseFloat(GAdditionalCharge) + list.Amount;

            }
            $scope.TotalFloorPlc = TotalFloorPlc;
            $scope.GTotalFloorPlc = GTotalFloorPlc;
            $scope.GAdditionalCharge = GAdditionalCharge;
            if ($scope.FlatDetails.FlatPlanCharge.length > 0)
                document.getElementById("planaf").innerHTML = parseFloat(parseFloat($scope.FlatDetails.FlatPlanCharge[0].AmountSqFt) * parseFloat($scope.FlatDetails.FlatPlanCharge[0].Size)).toFixed(0);
            if ($scope.FlatDetails.FlatPlanCharge.length > 1)
                document.getElementById("planauf").innerHTML = parseFloat(parseFloat($scope.FlatDetails.FlatPlanCharge[1].AmountSqFt) * parseFloat($scope.FlatDetails.FlatPlanCharge[1].Size)).toFixed(0);
            if ($scope.FlatDetails.FlatPlanCharge.length > 2)
                document.getElementById("planbf").innerHTML = parseFloat(parseFloat($scope.FlatDetails.FlatPlanCharge[2].AmountSqFt) * parseFloat($scope.FlatDetails.FlatPlanCharge[2].Size)).toFixed(0);
            if ($scope.FlatDetails.FlatPlanCharge.length > 3)
                document.getElementById("planbuf").innerHTML = parseFloat(parseFloat($scope.FlatDetails.FlatPlanCharge[3].AmountSqFt) * parseFloat($scope.FlatDetails.FlatPlanCharge[3].Size)).toFixed(0);
            if ($scope.FlatDetails.FlatPlanCharge.length > 4)
                document.getElementById("plancf").innerHTML = parseFloat(parseFloat($scope.FlatDetails.FlatPlanCharge[4].AmountSqFt) * parseFloat($scope.FlatDetails.FlatPlanCharge[4].Size)).toFixed(0);
            if ($scope.FlatDetails.FlatPlanCharge.length > 5)
                document.getElementById("plancuf").innerHTML = parseFloat(parseFloat($scope.FlatDetails.FlatPlanCharge[5].AmountSqFt) * parseFloat($scope.FlatDetails.FlatPlanCharge[5].Size)).toFixed(0);
            $("#loading").hide();
            $scope.TotalChargeAmount = totalCharge;
            $scope.GTotalChargeAmount = GtotalCharge;
            $scope.GTotal = $scope.GTotalChargeAmount + $scope.GTotalPLCAmount + $scope.FlatDetails.SalePrice;

        })
    }
    $scope.ShowPlanPrice = function (FType, Size) {
        $("#loading").show();
        var PlanName = $("#PlanID").find(":selected").text();
        $http({
            method: 'Get',
            url: '/Sale/Property/GetPlanTypeMasterByParams',
            params: { PlanName: PlanName, FType: FType, Size: Size }
        }).success(function (data) {
            if (data != "null") {
                $scope.GTotal = $scope.GTotalChargeAmount + $scope.GTotalPLCAmount + data.AmountSqFt * Size;
            }
            else {
                $("#MessageArea").show();
                $scope.MessageClass = "danger";
                $scope.MessageTitle = "Error";
                $scope.Message = "Installment Plan details not found.";
            }

            $("#loading").hide();
        }).error(function (error) {
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Installment Plan details not found.";
        })
    }

    //NewSale
    $scope.NewSaleInit = function () {
        $("#loading").show();
        $http({
            method: 'Get',
            url: '/Master/PlanCtrl/GetPlanTypeMasterList',
        }).success(function (data) {
            $scope.PlanTypeMasterList = data;
        })

        var flatid = $("#hidFlatID").val();
        $http({
            method: 'Get',
            url: '/Sale/Property/GetFlatDetails',
            params: { flatid: flatid },
        }).success(function (data) {
            $scope.FlatDetails = data;
            var totalPLC = 0;
            var GtotalPLC = 0;
            for (var i = 0; i < $scope.FlatDetails.FlatPLCList.length; i++) {
                var list = $scope.FlatDetails.FlatPLCList[i];
                totalPLC += list.AmountSqFt;
                GtotalPLC += list.TotalAmount;
            }
            $scope.TotalPLCAmount = totalPLC;
            $scope.GTotalPLCAmount = GtotalPLC;
            var totalCharge = 0;
            var GtotalCharge = 0;
            for (var i = 0; i < $scope.FlatDetails.FlatChargeList.length; i++) {
                var list = $scope.FlatDetails.FlatChargeList[i];
                totalCharge += list.Amount;
                GtotalCharge += list.TotalAmount;
            }
            $("#loading").hide();
            $scope.TotalChargeAmount = totalCharge;
            $scope.GTotalChargeAmount = GtotalCharge;
            $scope.GTotal = $scope.GTotalChargeAmount + $scope.GTotalPLCAmount + $scope.FlatDetails.SalePrice;
        })
    }

    $scope.GenerateInstallment = function () {

        var plc = $("#chkPLC").is(":checked");
        var acharge = $("#chkACharges").is(":checked");
        var aocharge = $("#chkAOCharges").is(":checked");

        var plcvalue = $("#txtPLC").val();
        var achargevalue = $("#txtACharges").val();
        var aochargevalue = $("#txtAOCharges").val();
        $http({
            method: 'Get',
            url: '/Sale/Property/GetInstallmentPlan',
            params: { PlanTypeMasterID: $scope.plan.PlanTypeMasterID, plc: plc, acharge: acharge, aocharge: aocharge, plcValue: plcvalue, achargevalue: achargevalue, aochargevalue: aochargevalue }
        }).success(function (data) {
            $("#planFoot").html(data[2]);
            $("#planHead").html(data[1]);
            $("#planbody").html(data[0]);
        })
    }

    $scope.AddRowClick = function () {
        $("#loading").show();
        var plc = $("#chkPLC").is(":checked");
        var acharge = $("#chkACharges").is(":checked");
        var aocharge = $("#chkAOCharges").is(":checked");

        $http({
            method: 'Get',
            url: '/Sale/Property/AddInstallmentPlanRow',
            params: { PlanTypeMasterID: $scope.plan.PlanTypeMasterID, plc: plc, acharge: acharge, aocharge: aocharge }
        }).success(function (data) {
            $("#planbody").append(data);
            $("#loading").hide();
        }).error(function (error) {
            $("#loading").hide();
            $("#MessageArea").show();
            $scope.MessageClass = "danger";
            $scope.MessageTitle = "Error";
            $scope.Message = "Installment plan row can't added, please validate information";
        })
    }

    $("#loading").hide();
})

function DeletePlanRow(rno) {
    $("#" + rno).remove();
}