# 🎹 MIDI Velocity and Dynamic Response Summary (Final Extended Version)

This summary details the non-linear relationship between MIDI Velocity ($v$) and the resulting Amplitude and Decibel ($\text{dB}$) level, using a standard power curve with a gamma ($\gamma$) of 2.0.

## I. Core Formulas

### 1. The Universal Decibel (dB) Formulas

The decibel is a logarithmic unit used to express the ratio between two values of a physical quantity, typically power or root-power (amplitude/pressure).

| Ratio Type | Formula | Description |
| :--- | :--- | :--- |
| **Power Ratio** | $\text{dB} = 10 \log_{10} \left(\frac{P_1}{P_0}\right)$ | Used for ratios of power (e.g., Watts). |
| **Root-Power/Amplitude Ratio** | $\text{dB} = 20 \log_{10} \left(\frac{A_1}{A_0}\right)$ | Used for ratios of amplitude or pressure (e.g., sound pressure level, voltage). |

### 2. MIDI Velocity-to-dB Conversion

The MIDI system converts velocity into a perceived $\text{dB}$ level relative to the maximum output (where $A_{\text{ref}} = 1.0$) by using a power function (controlled by $\gamma$) substituted into the $20 \log_{10}$ formula.

| Metric | Formula | Description |
| :--- | :--- | :--- |
| **MIDI Amplitude** | $\text{Amplitude} = \left(\frac{v}{127}\right)^\gamma$ | The instantaneous signal level (linear scale). |
| **General MIDI dB** | $\text{dB} = 20 \cdot \gamma \cdot \log_{10} \left(\frac{v}{127}\right)$ | The perceived loudness relative to max ($v=127$). |

* ***Simplified Formula Used in this Analysis:*** With the common standard $\mathbf{\gamma = 2.0}$ (the quadratic curve), the formula is simplified to: $\mathbf{\text{dB} = 40 \log_{10} \left(\frac{v}{127}\right)}$.

## II. Roland Dynamic Velocity Analysis (Extended)

Roland's classic velocity values establish a musically useful hierarchy of loudness using the $\gamma=2.0$ curve.

| Velocity ($v$) | Designation | Calculated Decibels ($\text{dB}$) | Dynamic Role |
| :---: | :---: | :---: | :--- |
| **127** | Full / Maximum | $\mathbf{0.00\,\text{dB}}$ | Absolute Loudest Point (Reference) |
| **80** | **Accented** | $\mathbf{-4.00\,\text{dB}}$ | Strong, emphasized rhythmic beat. |
| **50** | **Normal** | $\mathbf{-12.16\,\text{dB}}$ | Standard, consistent rhythmic beat. |
| **35** | **Soft Ghost** | $\mathbf{-18.23\,\text{dB}}$ | Audible rhythm texture; provides clear groove. |
| **15** | **Deep Ghost** | $\mathbf{-30.76\,\text{dB}}$ | Barely audible, subsurface texture; adds "sizzle." |

### 🔑 Key Dynamic Separations (Acoustic Impact)

The $\text{dB}$ difference is what matters for human perception:

| Dynamic Step | $\Delta v$ | $\Delta \text{dB}$ | Perceived Loudness |
| :--- | :---: | :---: | :--- |
| **Accented $\to$ Normal** | $80 \to 50$ | $\mathbf{8.16\,\text{dB}}$ | Provides the fundamental "pop" of the rhythm. |
| **Normal $\to$ Soft Ghost** | $50 \to 35$ | $\mathbf{6.07\,\text{dB}}$ | Approximately a $6\,\text{dB}$ drop, perceived as half the power. |
| **Soft Ghost $\to$ Deep Ghost** | $35 \to 15$ | $\mathbf{12.53\,\text{dB}}$ | Drops the note deep into the mix, making it almost purely background noise/texture. |