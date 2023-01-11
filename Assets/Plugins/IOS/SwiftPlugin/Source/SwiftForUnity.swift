//
//  SwiftForUnity.swift
//  SwiftPlugin
//
//  Created by Monis on 17/01/2020.
//  Copyright © 2019 Monis. All rights reserved.
//

import Foundation
import HealthKit

let store:HKHealthStore = HKHealthStore()

@objc public class SwiftForUnity: UIViewController {
    
    //let storage = HKHealthStore()
    let healthStore = HKHealthStore()
      @objc static let shared = SwiftForUnity()
    let viewController =  UIApplication.shared.keyWindow?.rootViewController
    
      @objc func getStepCount(Days: Int){
        var stepCount="NO DATA";
       // importFiles()
        if(self.getHealthKitPermission()){
        self.getTodaysSteps(from: Days) { (steps) in
                UnitySendMessage("Callback", "OnRecieveStepCountData",String(Days) + "-" + String(format: "%.0f", steps))
                //return stepCount = String(format: "%.0f", steps);
               // print(stepCount)
         }
        }else{
            UnitySendMessage("Callback", "OnRecieveStepCountData","No Permission To Access")
        }
    }
    
    func getHealthKitPermission() -> Bool
        {
            var isEnabled = true
//        guard HKHealthStore.isHealthDataAvailable() else {
//            UnitySendMessage("Callback", "OnRecieveFileURL","Health Data not available.")
//                return
//            }

            let stepsCount = HKObjectType.quantityType(forIdentifier: HKQuantityTypeIdentifier.stepCount)!

            healthStore.requestAuthorization(toShare: [], read: [stepsCount]) { (success, error) in
                if success {
                    print("Permission accept.")
                    //UnitySendMessage("Callback", "OnRecieveFileURL","Permission accept.")
                    isEnabled = true
                }
                else {
                    if error != nil {
                        print(error ?? "")
                    }
                    isEnabled = false
                   // UnitySendMessage("Callback", "OnRecieveFileURL","Permission denied.")
                   // return false
                }
            }
            return isEnabled
        }

    func getTodaysSteps(from days: Int,completion: @escaping (Double) -> Void) {
        let stepsQuantityType = HKQuantityType.quantityType(forIdentifier: .stepCount)!
        
        var toDate = Date()
        var fromDate = Calendar.current.startOfDay(for: toDate)
        if(days != 0)
        {
            let now = Calendar.current.dateComponents(in: .current, from: Date())
            let forDay = DateComponents(year: now.year, month: now.month, day: now.day! - days)
            let dateforDay = Calendar.current.date(from: forDay)!
            fromDate = Calendar.current.startOfDay(for: dateforDay)
            toDate = Calendar.current.date(bySettingHour: 23, minute: 59, second: 59, of: dateforDay)!
        }
        let predicate = HKQuery.predicateForSamples(withStart: fromDate, end: toDate, options: .strictStartDate)

        let query = HKStatisticsQuery(quantityType: stepsQuantityType, quantitySamplePredicate: predicate, options: .cumulativeSum) { _, result, _ in
            guard let result = result, let sum = result.sumQuantity() else {
                completion(0.0)
                return
            }
            completion(sum.doubleValue(for: HKUnit.count()))
        }

        healthStore.execute(query)
    }
}


