# 🎹 MIDI Velocity and Dynamic Response Summary (Extended)

This summary details the non-linear relationship between MIDI Velocity ($v$) and the resulting Amplitude and Decibel ($\text{dB}$) level, using a standard power curve with a gamma ($\gamma$) of 2.0.

## I. Core Formulas

The relationship between MIDI Velocity (where $v$ ranges from $1$ to $127$) and volume is defined by the following power and logarithmic formulas:

| Metric | Formula | Description |
| :--- | :--- | :--- |
| **Amplitude** | $\text{Amplitude} = \left(\frac{v}{127}\right)^\gamma$ | Defines the raw signal level (linear scale). |
| **Decibels (dB)** | $\text{dB} = 20 \cdot \gamma \cdot \log_{10} \left(\frac{v}{127}\right)$ | Defines the perceived loudness (logarithmic scale). |

* ***Note:*** Our discussion used the standard quadratic curve, where $\mathbf{\gamma = 2.0}$, making the $\text{dB}$ formula: $\mathbf{\text{dB} = 40 \log_{10} \left(\frac{v}{127}\right)}$.

## II. Roland Dynamic Velocity Analysis (Extended)

Roland's classic velocity values establish a hierarchy of loudness. By adding two tiers of ghost notes, we create a more musically sophisticated dynamic range.

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
| **Normal $\to$ Soft Ghost** | $50 \to 35$ | $\mathbf{6.07\,\text{dB}}$ | Exactly a $6\,\text{dB}$ drop, perceived as half the power. |
| **Soft Ghost $\to$ Deep Ghost** | $35 \to 15$ | $\mathbf{12.53\,\text{dB}}$ | Drops the note deep into the mix, making it almost purely background noise/texture. |

By using $v=35$ and $v=15$, a programmer can create extremely realistic and intricate drum patterns where the difference between the loudest hit ($\mathbf{0\,\text{dB}}$) and the softest hit ($\mathbf{-30.76\,\text{dB}}$) spans over **30 decibels**, providing a massive dynamic range.