//
//  UnityRuntimeObject.m
//  Unity-iPhone
//
//  Created by elon on 2021/8/29.
//

#import "UnityRuntimeObject.h"

@implementation UnityRuntimeObject

+ (UIViewController *)getGLViewController{
    return UnityGetGLViewController();
}

+ (UIView *)getGLView{
    return UnityGetGLView();
}

+ (void)sendMessageTest {
    UnitySendMessage("Canvas", "AcceptData_Android", "{\"name\":\"张大牛\",\"goto\":\"雇主\",\"invite_code\":\"1234567\",\"invite_type\":\"type\",\"cust_name\":\"某某某公司\"}");
}

+ (void)sendMethod:(NSString *)method message:(NSString *)message {
    UnitySendMessage("Canvas", method.UTF8String, message.UTF8String);
}

@end
