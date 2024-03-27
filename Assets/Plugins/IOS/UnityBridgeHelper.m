#import <Foundation/Foundation.h>
#import <DeviceCheck/DeviceCheck.h>
    void _FetchDeviceCheckToken() {
        if (@available(iOS 12.0, *)) {
            DCDevice *device = [DCDevice currentDevice];
            [device generateTokenWithCompletionHandler:^(NSData * _Nullable token, NSError * _Nullable error) {
                if (token) {
                    NSString *base64EncodedToken = [token base64EncodedStringWithOptions:0];
                    const char* tokenCString = [base64EncodedToken UTF8String];
                    UnitySendMessage("DeviceCheckTokenListener", "ReceiveDeviceCheckToken", tokenCString);
                } else {
                    UnitySendMessage("DeviceCheckTokenListener", "ReceiveDeviceCheckToken", "");
                }
            }];
        } else {
            UnitySendMessage("DeviceCheckTokenListener", "ReceiveDeviceCheckToken", "");
        }
    }

