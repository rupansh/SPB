﻿using SPB.Graphics;
using SPB.Graphics.Exceptions;
using SPB.Graphics.OpenGL;
using SPB.Platform.GLX;
using SPB.Platform.X11;
using SPB.Platform.WGL;
using SPB.Platform.Win32;
using SPB.Windowing;
using System;
using System.Runtime.InteropServices;

namespace SPB.Platform
{
    public sealed class PlatformHelper
    {
        public static SwapableNativeWindowBase CreateOpenGLWindow(FramebufferFormat format, int x, int y, int width, int height)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // TODO: detect X11/Wayland/DRI
                return X11Helper.CreateGLXWindow(new NativeHandle(X11.X11.DefaultDisplay), format, x, y, width, height);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // TODO pass format
                return Win32Helper.CreateWindowForWGL(x, y, width, height);
            }

            throw new NotImplementedException();
        }

        public static OpenGLContextBase CreateOpenGLContext(FramebufferFormat framebufferFormat, int major, int minor, OpenGLContextFlags flags = OpenGLContextFlags.Default, bool directRendering = true, OpenGLContextBase shareContext = null)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // TODO: detect X11/Wayland/DRI
                if (shareContext != null && !(shareContext is GLXOpenGLContext))
                {
                    throw new ContextException($"shared context must be of type {typeof(GLXOpenGLContext).Name}.");
                }

                return new GLXOpenGLContext(framebufferFormat, major, minor, flags, directRendering, (GLXOpenGLContext)shareContext);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (shareContext != null && !(shareContext is WGLOpenGLContext))
                {
                    throw new ContextException($"shared context must be of type {typeof(WGLOpenGLContext).Name}.");
                }

                return new WGLOpenGLContext(framebufferFormat, major, minor, flags, directRendering, (WGLOpenGLContext)shareContext);
            }

            throw new NotImplementedException();
        }
    }
}
