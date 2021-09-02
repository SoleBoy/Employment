//
//  UnityRuntimeObject.h
//  Unity-iPhone
//
//  Created by elon on 2021/8/29.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface UnityRuntimeObject : NSObject

+ (UIViewController *)getGLViewController;
+ (UIView *)getGLView;

+ (void)sendMessageTest;

+ (void)sendMethod:(NSString *)method message:(NSString *)message;
@end

NS_ASSUME_NONNULL_END
